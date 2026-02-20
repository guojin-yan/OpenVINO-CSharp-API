// Copyright (c) 2024 Guojin Yan
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using OpenVinoSharp.Internal;

namespace OpenVinoSharp
{
    /// <summary>
    /// 编译模型缓存 / Compiled Model Cache
    /// <para>缓存已编译的模型，避免重复编译，提高多推理场景的性能。/ Caches compiled models to avoid recompilation and improve performance in multi-inference scenarios.</para>
    /// </summary>
    /// <example>
    /// 使用示例 / Usage example:
    /// <code>
    /// // 配置缓存 / Configure cache
    /// ModelCache.Enabled = true;
    /// ModelCache.MaxCacheSize = 5;
    /// 
    /// // 使用Core加载模型，缓存会自动生效 / Use Core to load model, cache will be applied automatically
    /// using (Core core = new Core())
    /// {
    ///     var model = core.read_model("model.xml");
    ///     var compiled = core.compile_model(model, "CPU"); // 首次编译 / First compilation
    /// }
    /// 
    /// // 清空缓存 / Clear cache
    /// ModelCache.Clear();
    /// </code>
    /// </example>
    public static class ModelCache
    {
        // 缓存字典：Key = 模型标识, Value = (编译模型, 引用计数)
        // Cache dictionary: Key = model identifier, Value = (compiled model, reference count)
        private static readonly Dictionary<string, CacheEntry> _cache = new Dictionary<string, CacheEntry>();
        private static readonly object _lock = new object();
        private static bool _enabled = true;
        private static int _maxCacheSize = 10;

        /// <summary>
        /// 缓存条目 / Cache entry
        /// </summary>
        private class CacheEntry
        {
            public CompiledModel Model { get; set; }
            public int ReferenceCount { get; set; }
            public DateTime LastAccessTime { get; set; }
            public string DeviceName { get; set; }
            public string PropertiesHash { get; set; }
        }

        /// <summary>
        /// 获取或设置缓存是否启用 / Gets or sets whether the cache is enabled
        /// </summary>
        /// <value>是否启用 / Whether enabled</value>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// ModelCache.Enabled = true;  // 启用缓存 / Enable cache
        /// ModelCache.Enabled = false; // 禁用缓存 / Disable cache
        /// </code>
        /// </example>
        public static bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        /// <summary>
        /// 获取或设置最大缓存大小 / Gets or sets the maximum cache size
        /// </summary>
        /// <value>最大缓存条目数 / Maximum number of cache entries</value>
        /// <exception cref="ArgumentException">当值小于1时抛出 / Thrown when value is less than 1</exception>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// ModelCache.MaxCacheSize = 10; // 最多缓存10个模型 / Cache up to 10 models
        /// </code>
        /// </example>
        public static int MaxCacheSize
        {
            get => _maxCacheSize;
            set
            {
                if (value < 1)
                    throw new ArgumentException("最大缓存大小必须大于0 / Maximum cache size must be greater than 0");
                _maxCacheSize = value;
            }
        }

        /// <summary>
        /// 尝试从缓存获取编译模型 / Try to get compiled model from cache
        /// </summary>
        /// <param name="modelPath">模型路径 / Model path</param>
        /// <param name="deviceName">设备名称 / Device name</param>
        /// <param name="properties">编译属性 / Compilation properties</param>
        /// <returns>缓存的编译模型，如果不存在则返回 null / Cached compiled model, or null if not found</returns>
        internal static CompiledModel TryGet(string modelPath, string deviceName, Dictionary<string, string> properties)
        {
            if (!_enabled)
                return null;

            string key = GenerateCacheKey(modelPath, deviceName, properties);

            lock (_lock)
            {
                if (_cache.TryGetValue(key, out var entry))
                {
                    // 检查模型是否已被释放 / Check if model has been disposed
                    if (entry.Model == null || entry.Model.IsDisposed)
                    {
                        _cache.Remove(key);
                        return null;
                    }

                    entry.ReferenceCount++;
                    entry.LastAccessTime = DateTime.Now;
                    Logger.Debug($"ModelCache: 命中缓存 / Cache hit - {modelPath} [{deviceName}]");
                    return entry.Model;
                }
            }

            return null;
        }

        /// <summary>
        /// 将编译模型添加到缓存 / Add compiled model to cache
        /// </summary>
        /// <param name="modelPath">模型路径 / Model path</param>
        /// <param name="deviceName">设备名称 / Device name</param>
        /// <param name="properties">编译属性 / Compilation properties</param>
        /// <param name="compiledModel">编译后的模型 / Compiled model</param>
        internal static void Add(string modelPath, string deviceName, Dictionary<string, string> properties, CompiledModel compiledModel)
        {
            if (!_enabled || compiledModel == null)
                return;

            string key = GenerateCacheKey(modelPath, deviceName, properties);

            lock (_lock)
            {
                // 如果缓存已满，移除最久未访问的条目 / If cache is full, evict least recently used entry
                if (_cache.Count >= _maxCacheSize)
                {
                    EvictLRU();
                }

                _cache[key] = new CacheEntry
                {
                    Model = compiledModel,
                    ReferenceCount = 1,
                    LastAccessTime = DateTime.Now,
                    DeviceName = deviceName,
                    PropertiesHash = GetPropertiesHash(properties)
                };

                Logger.Debug($"ModelCache: 添加缓存 / Cache added - {modelPath} [{deviceName}], 当前缓存大小 / Current cache size: {_cache.Count}");
            }
        }

        /// <summary>
        /// 释放缓存中的模型（减少引用计数）/ Release model from cache (decrement reference count)
        /// </summary>
        /// <param name="modelPath">模型路径 / Model path</param>
        /// <param name="deviceName">设备名称 / Device name</param>
        /// <param name="properties">编译属性 / Compilation properties</param>
        internal static void Release(string modelPath, string deviceName, Dictionary<string, string> properties)
        {
            if (!_enabled)
                return;

            string key = GenerateCacheKey(modelPath, deviceName, properties);

            lock (_lock)
            {
                if (_cache.TryGetValue(key, out var entry))
                {
                    entry.ReferenceCount--;
                    
                    // 引用计数为0时不立即释放，等待 LRU 淘汰 / Don't release immediately when ref count is 0, wait for LRU eviction
                    Logger.Debug($"ModelCache: 释放引用 / Release reference - {modelPath} [{deviceName}], 剩余引用 / Remaining refs: {entry.ReferenceCount}");
                }
            }
        }

        /// <summary>
        /// 清空缓存 / Clear the cache
        /// </summary>
        /// <example>
        /// 使用示例 / Usage example:
        /// <code>
        /// // 清空所有缓存的模型 / Clear all cached models
        /// ModelCache.Clear();
        /// Console.WriteLine($"缓存大小: {ModelCache.Count}"); // 输出 / Output: 0
        /// </code>
        /// </example>
        public static void Clear()
        {
            lock (_lock)
            {
                foreach (var entry in _cache.Values)
                {
                    try
                    {
                        entry.Model?.Dispose();
                    }
                    catch { }
                }
                _cache.Clear();
                Logger.Info("ModelCache: 缓存已清空 / Cache cleared");
            }
        }

        /// <summary>
        /// 获取当前缓存大小 / Get current cache size
        /// </summary>
        /// <value>缓存中的条目数 / Number of entries in cache</value>
        public static int Count
        {
            get
            {
                lock (_lock)
                {
                    return _cache.Count;
                }
            }
        }

        /// <summary>
        /// 生成缓存键 / Generate cache key
        /// </summary>
        /// <param name="modelPath">模型路径 / Model path</param>
        /// <param name="deviceName">设备名称 / Device name</param>
        /// <param name="properties">编译属性 / Compilation properties</param>
        /// <returns>缓存键 / Cache key</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GenerateCacheKey(string modelPath, string deviceName, Dictionary<string, string> properties)
        {
            // 使用模型路径的绝对路径 + 设备名 + 属性哈希 / Use absolute model path + device name + properties hash
            string fullPath = System.IO.Path.GetFullPath(modelPath).ToLowerInvariant();
            string propsHash = GetPropertiesHash(properties);
            return $"{fullPath}|{deviceName.ToLowerInvariant()}|{propsHash}";
        }

        /// <summary>
        /// 计算属性字典的哈希值 / Calculate hash of properties dictionary
        /// </summary>
        /// <param name="properties">属性字典 / Properties dictionary</param>
        /// <returns>哈希字符串 / Hash string</returns>
        private static string GetPropertiesHash(Dictionary<string, string> properties)
        {
            if (properties == null || properties.Count == 0)
                return "empty";

            // 按键排序后计算哈希 / Calculate hash after sorting by key
            var sorted = new SortedDictionary<string, string>(properties);
            using (var sha256 = SHA256.Create())
            {
                var sb = new StringBuilder();
                foreach (var pair in sorted)
                {
                    sb.Append(pair.Key).Append('=').Append(pair.Value).Append(';');
                }
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                return BytesToHex(hash, 16);
            }
        }

        /// <summary>
        /// 将字节数组转换为十六进制字符串（兼容 .NET Framework）/ Convert byte array to hexadecimal string (.NET Framework compatible)
        /// </summary>
        /// <param name="bytes">字节数组 / Byte array</param>
        /// <param name="maxLength">最大长度 / Maximum length</param>
        /// <returns>十六进制字符串 / Hexadecimal string</returns>
        private static string BytesToHex(byte[] bytes, int maxLength)
        {
            var sb = new StringBuilder(Math.Min(maxLength * 2, bytes.Length * 2));
            for (int i = 0; i < Math.Min(bytes.Length, maxLength); i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 淘汰最久未访问的条目 / Evict least recently used entry
        /// </summary>
        private static void EvictLRU()
        {
            DateTime oldest = DateTime.MaxValue;
            string keyToRemove = null;

            foreach (var pair in _cache)
            {
                // 优先淘汰引用计数为0的 / Prioritize entries with reference count of 0
                if (pair.Value.ReferenceCount == 0 && pair.Value.LastAccessTime < oldest)
                {
                    oldest = pair.Value.LastAccessTime;
                    keyToRemove = pair.Key;
                }
            }

            // 如果没有引用计数为0的，淘汰最久未访问的 / If no entries with ref count 0, evict least recently accessed
            if (keyToRemove == null)
            {
                foreach (var pair in _cache)
                {
                    if (pair.Value.LastAccessTime < oldest)
                    {
                        oldest = pair.Value.LastAccessTime;
                        keyToRemove = pair.Key;
                    }
                }
            }

            if (keyToRemove != null)
            {
                var entry = _cache[keyToRemove];
                try
                {
                    entry.Model?.Dispose();
                }
                catch { }
                _cache.Remove(keyToRemove);
                Logger.Debug($"ModelCache: LRU 淘汰 / LRU evicted - {keyToRemove}");
            }
        }
    }
}
