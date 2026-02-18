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
    public static class ModelCache
    {
        // 缓存字典：Key = 模型标识, Value = (编译模型, 引用计数)
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
        /// 获取或设置缓存是否启用
        /// </summary>
        public static bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        /// <summary>
        /// 获取或设置最大缓存大小
        /// </summary>
        public static int MaxCacheSize
        {
            get => _maxCacheSize;
            set
            {
                if (value < 1)
                    throw new ArgumentException("最大缓存大小必须大于0");
                _maxCacheSize = value;
            }
        }

        /// <summary>
        /// 尝试从缓存获取编译模型
        /// </summary>
        /// <param name="modelPath">模型路径</param>
        /// <param name="deviceName">设备名称</param>
        /// <param name="properties">编译属性</param>
        /// <returns>缓存的编译模型，如果不存在则返回 null</returns>
        internal static CompiledModel TryGet(string modelPath, string deviceName, Dictionary<string, string> properties)
        {
            if (!_enabled)
                return null;

            string key = GenerateCacheKey(modelPath, deviceName, properties);

            lock (_lock)
            {
                if (_cache.TryGetValue(key, out var entry))
                {
                    // 检查模型是否已被释放
                    if (entry.Model == null || entry.Model.IsDisposed)
                    {
                        _cache.Remove(key);
                        return null;
                    }

                    entry.ReferenceCount++;
                    entry.LastAccessTime = DateTime.Now;
                    Logger.Debug($"ModelCache: 命中缓存 - {modelPath} [{deviceName}]");
                    return entry.Model;
                }
            }

            return null;
        }

        /// <summary>
        /// 将编译模型添加到缓存
        /// </summary>
        /// <param name="modelPath">模型路径</param>
        /// <param name="deviceName">设备名称</param>
        /// <param name="properties">编译属性</param>
        /// <param name="compiledModel">编译后的模型</param>
        internal static void Add(string modelPath, string deviceName, Dictionary<string, string> properties, CompiledModel compiledModel)
        {
            if (!_enabled || compiledModel == null)
                return;

            string key = GenerateCacheKey(modelPath, deviceName, properties);

            lock (_lock)
            {
                // 如果缓存已满，移除最久未访问的条目
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

                Logger.Debug($"ModelCache: 添加缓存 - {modelPath} [{deviceName}], 当前缓存大小: {_cache.Count}");
            }
        }

        /// <summary>
        /// 释放缓存中的模型（减少引用计数）
        /// </summary>
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
                    
                    // 引用计数为0时不立即释放，等待 LRU 淘汰
                    Logger.Debug($"ModelCache: 释放引用 - {modelPath} [{deviceName}], 剩余引用: {entry.ReferenceCount}");
                }
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
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
                Logger.Info("ModelCache: 缓存已清空");
            }
        }

        /// <summary>
        /// 获取当前缓存大小
        /// </summary>
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
        /// 生成缓存键
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GenerateCacheKey(string modelPath, string deviceName, Dictionary<string, string> properties)
        {
            // 使用模型路径的绝对路径 + 设备名 + 属性哈希
            string fullPath = System.IO.Path.GetFullPath(modelPath).ToLowerInvariant();
            string propsHash = GetPropertiesHash(properties);
            return $"{fullPath}|{deviceName.ToLowerInvariant()}|{propsHash}";
        }

        /// <summary>
        /// 计算属性字典的哈希值
        /// </summary>
        private static string GetPropertiesHash(Dictionary<string, string> properties)
        {
            if (properties == null || properties.Count == 0)
                return "empty";

            // 按键排序后计算哈希
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
        /// 将字节数组转换为十六进制字符串（兼容 .NET Framework）
        /// </summary>
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
        /// 淘汰最久未访问的条目
        /// </summary>
        private static void EvictLRU()
        {
            DateTime oldest = DateTime.MaxValue;
            string keyToRemove = null;

            foreach (var pair in _cache)
            {
                // 优先淘汰引用计数为0的
                if (pair.Value.ReferenceCount == 0 && pair.Value.LastAccessTime < oldest)
                {
                    oldest = pair.Value.LastAccessTime;
                    keyToRemove = pair.Key;
                }
            }

            // 如果没有引用计数为0的，淘汰最久未访问的
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
                Logger.Debug($"ModelCache: LRU 淘汰 - {keyToRemove}");
            }
        }
    }
}
