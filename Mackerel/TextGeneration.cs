using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mackerel;

public static partial class Macro
{
    private static readonly MemoryCache _ConstantCache = MemoryCache.Default;

    private static readonly CacheItemPolicy _CacheItemPolicy = new()
    {
        SlidingExpiration = TimeSpan.FromMinutes(1)
    };

    private static readonly Regex Whitespace = WhitespaceRegex();

    public static string Empty(in Document _) => "";

    public static Instruction Str(object value) => (in Document _) => value.ToString() ?? "";

    public static Instruction LongText(string text) =>
        (in Document _) => 
            Whitespace.Replace(text.Replace(Environment.NewLine, " "), " ").Trim();

    public static Instruction Open(string path) =>
        (in Document doc) =>
        {
            if (!File.Exists(path))
                return "";

            return File.ReadAllText(path);
        };

    public static Instruction Text(string text)
    {
        if (_ConstantCache.Contains(text))
            return (Instruction)_ConstantCache[text];

        Instruction instr = (in Document _) => text;

        _ConstantCache.Set(text, instr, _CacheItemPolicy);

        return instr;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}
