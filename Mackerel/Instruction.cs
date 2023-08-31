using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mackerel;

public delegate string Instruction(in Document ctx);


public static partial class Macro
{
    public static Instruction Bind(Instruction instr, Func<string, Instruction> bind) =>
        (in Document ctx) => bind(instr(in ctx))(in ctx);

    public static Instruction Block(params Instruction[] instrs) =>
        (in Document ctx) =>
        {
            for (var i = 0; i < instrs.Length - 1; i++)
                instrs[i](in ctx);

            return instrs.Length > 0 ? instrs[^1](in ctx) : "";
        };

    public static Instruction If(Func<string, bool> test, Instruction value, Instruction then, Instruction alt) =>
        (in Document ctx) =>
            test(value(in ctx)) ? then(in ctx) : alt(in ctx);

    public static Instruction If<T>(Func<T, bool> test, T value, Instruction then, Instruction alt) =>
        (in Document ctx) => test(value) ? then(in ctx) : alt(in ctx);

    public static Instruction IfN(Func<string, bool> test, Instruction value, Instruction then, Instruction alt) =>
        (in Document ctx) =>
            !test(value(in ctx)) ? then(in ctx) : alt(in ctx);

    public static Instruction IfN<T>(Func<T, bool> test, T value, Instruction then, Instruction alt) =>
        (in Document ctx) => !test(value) ? then(in ctx) : alt(in ctx);

    public static Instruction Map(Instruction instr, Func<string, string> map) =>
        (in Document ctx) => map(instr(in ctx));

    public static Instruction Mutate<T>(T value, Func<T, Instruction> f) =>
        (in Document ctx) => f(value)(in ctx);

    public static Instruction Mutate<T>(Instruction instr, Func<string, T> map, Func<T, Instruction> bind) =>
        (in Document ctx) => bind(map(instr(in ctx)))(in ctx);
}
