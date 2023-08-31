using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Mackerel;

public static partial class Macro
{
    private static readonly MemoryCache _ConstantCache = MemoryCache.Default;

    private static readonly CacheItemPolicy _CacheItemPolicy = new CacheItemPolicy()
    {
        SlidingExpiration = TimeSpan.FromMinutes(1)
    };

    public static string Empty(in Document _) => "";

    public static Instruction Str(object value) => (in Document _) => value.ToString() ?? "";

    public static Instruction Text(string text)
    {
        if (_ConstantCache.Contains(text))
            return (Instruction)_ConstantCache[text];

        Instruction instr = (in Document _) => text;

        _ConstantCache.Set(text, instr, _CacheItemPolicy);

        return instr;
    }
}
