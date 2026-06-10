// Copyright (c) 2026 Guojin Yan
// Licensed under the Apache-2.0 License.

using System.Globalization;

namespace GenAI.Common;

/// <summary>
/// Small command-line option helper used by GenAI samples.
/// GenAI 示例使用的轻量命令行参数解析工具。
/// </summary>
public sealed class SampleOptions
{
    private readonly Dictionary<string, string> _values;
    private readonly Dictionary<string, List<string>> _allValues;

    private SampleOptions(Dictionary<string, string> values, Dictionary<string, List<string>> allValues)
    {
        _values = values;
        _allValues = allValues;
    }

    /// <summary>
    /// Parses --key value and --key=value arguments.
    /// 解析 --key value 和 --key=value 参数。
    /// </summary>
    public static SampleOptions Parse(string[] args)
    {
        Dictionary<string, string> values = new(StringComparer.OrdinalIgnoreCase);
        Dictionary<string, List<string>> allValues = new(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];
            if (!arg.StartsWith("-", StringComparison.Ordinal))
                continue;

            string key;
            string value;
            int equalsIndex = arg.IndexOf('=');
            if (equalsIndex > 0)
            {
                key = NormalizeKey(arg.Substring(0, equalsIndex));
                value = arg.Substring(equalsIndex + 1);
            }
            else
            {
                key = NormalizeKey(arg);
                if (i + 1 < args.Length && !args[i + 1].StartsWith("-", StringComparison.Ordinal))
                    value = args[++i];
                else
                    value = "true";
            }

            values[key] = value;
            if (!allValues.TryGetValue(key, out List<string>? keyValues))
            {
                keyValues = new List<string>();
                allValues[key] = keyValues;
            }
            keyValues.Add(value);
        }

        return new SampleOptions(values, allValues);
    }

    /// <summary>
    /// Gets a string option, optionally falling back to an environment variable.
    /// 获取字符串参数，可回退到环境变量。
    /// </summary>
    public string? Get(string key, string? defaultValue = null, string? env = null)
    {
        if (_values.TryGetValue(NormalizeKey(key), out string? value))
            return value;

        if (!string.IsNullOrEmpty(env))
        {
            string? envValue = Environment.GetEnvironmentVariable(env);
            if (!string.IsNullOrWhiteSpace(envValue))
                return envValue;
        }

        return defaultValue;
    }

    /// <summary>
    /// Gets a required string option or throws a helpful error.
    /// 获取必填字符串参数，缺失时抛出清晰错误。
    /// </summary>
    public string Require(string key, string? env = null)
    {
        string? value = Get(key, env: env);
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Missing required option --{NormalizeKey(key)}" + (env == null ? "." : $" or environment variable {env}."));

        return value;
    }

    /// <summary>
    /// Gets an integer option.
    /// 获取整数参数。
    /// </summary>
    public int GetInt(string key, int defaultValue, string? env = null)
    {
        string? value = Get(key, env: env);
        return string.IsNullOrWhiteSpace(value) ? defaultValue : int.Parse(value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets an unsigned long option.
    /// 获取无符号整数参数。
    /// </summary>
    public ulong GetUInt64(string key, ulong defaultValue, string? env = null)
    {
        string? value = Get(key, env: env);
        return string.IsNullOrWhiteSpace(value) ? defaultValue : ulong.Parse(value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets a floating-point option.
    /// 获取浮点参数。
    /// </summary>
    public float GetFloat(string key, float defaultValue, string? env = null)
    {
        string? value = Get(key, env: env);
        return string.IsNullOrWhiteSpace(value) ? defaultValue : float.Parse(value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets a boolean option.
    /// 获取布尔参数。
    /// </summary>
    public bool GetBool(string key, bool defaultValue, string? env = null)
    {
        string? value = Get(key, env: env);
        return string.IsNullOrWhiteSpace(value) ? defaultValue : bool.Parse(value);
    }

    /// <summary>
    /// Checks whether an option was supplied.
    /// 检查是否传入参数。
    /// </summary>
    public bool Has(string key) => _values.ContainsKey(NormalizeKey(key));

    /// <summary>
    /// Gets every value supplied for a repeatable option.
    /// 获取可重复参数的全部值。
    /// </summary>
    public IReadOnlyList<string> GetAll(string key)
    {
        return _allValues.TryGetValue(NormalizeKey(key), out List<string>? values)
            ? values
            : Array.Empty<string>();
    }

    private static string NormalizeKey(string key)
    {
        return key.TrimStart('-').Replace('_', '-');
    }
}
