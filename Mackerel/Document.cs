using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mackerel;

public class Document
{
    public static readonly Document Empty = new();

    private const int DEFAULT_BUFFER_COUNT = 10;

    private readonly IDictionary<string, object> _env;
    private readonly StringBuilder?[] _buffers;
    private int _buffer;
    private int _indent;

    public Document(int bufferCount = DEFAULT_BUFFER_COUNT)
    {
        _env = new Dictionary<string, object>();

        _buffers = new StringBuilder?[bufferCount < 1 ? DEFAULT_BUFFER_COUNT : bufferCount];
        _buffer = 0;
        _indent = 0;

    }

    public Document(IEnumerable<KeyValuePair<string, object>> env, int bufferCount = DEFAULT_BUFFER_COUNT)
    {
        _env = new Dictionary<string, object>(env);

        _buffers = new StringBuilder?[bufferCount < 1 ? DEFAULT_BUFFER_COUNT : bufferCount];
        _buffer = 0;
        _indent = 0;
    }

    internal void Clear()
    {
        _env.Clear();

        foreach (var builder in _buffers)
            builder?.Clear();

        _buffer = 0;
    }

    internal void Dedent(int indent)
    {
        if (indent > 0 && indent <= _indent)
            _indent -= indent;
    }

    internal bool Defines(string key) => _env.ContainsKey(key);

    internal void Include(params string[] paths)
    {
        if (_buffer < 0)
            return;

        if (_buffers[_buffer] is null)
            _buffers[_buffer] = new StringBuilder();

        foreach (var path in paths)
            if (File.Exists(path))
                _buffers[_buffer]!.AppendLine(File.ReadAllText(path));
    }

    internal void Indent(int indent)
    {
        if (indent > 0)
            _indent += indent;
    }

    internal string Read(params int[] buffers)
    {
        if (buffers.Length == 0)
            return _buffer >= 0 ? _buffers[_buffer]?.ToString() ?? "" : "";

        var read = new StringBuilder();

        foreach (var buffer in buffers)
            if (buffer >= 0 && buffer < _buffers.Length)
                read.Append(_buffers[buffer]?.ToString() ?? "");
        
        return read.ToString();
    }

    internal string ReadLine(params int[] buffers)
    {
        if (buffers.Length == 0)
            return $"{_buffers[0]?.ToString() ?? ""}{ Environment.NewLine}";

        var read = new StringBuilder();

        foreach (var buffer in buffers)
            if (buffer >= 0 && buffer < _buffers.Length)
                read.AppendLine(_buffers[buffer]?.ToString() ?? "");

        return read.ToString();
    }

    internal void Set<T>(string key, T value) where T : notnull
    {
        if (!_env.ContainsKey(key))
            _env[key] = value;
    }

    internal string Stringify(string name) => 
        _env.TryGetValue(name, out var value) ? value.ToString() ?? "" : "";

    internal bool TryGet<T>(string key, out T? value)
    {
        value = default;

        if (!_env.TryGetValue(key, out var obj) || obj is not T tvalue)
            return false;

        value = tvalue;

        return true;
    }

    internal void Unset(string key) => _env.Remove(key);

    internal void Update<T>(string key, T value) where T : notnull
    {
        if (_env.ContainsKey(key))
            _env[key] = value;
    }

    internal void Write(string value)
    {
        if (_buffer < 0)
            return;
        else if (_buffers[_buffer] is null)
            _buffers[_buffer] = new StringBuilder(value.PadLeft(value.Length + _indent));
        else
            _buffers[_buffer]!.Append(value.PadLeft(value.Length + _indent));
    }

    internal void WriteTo(int buffer) =>
        _buffer = buffer < 0 || buffer >= _buffers.Length ? -1 : buffer;
    
}
