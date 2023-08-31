using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mackerel;

public static partial class Macro
{
    public static Instruction Buffer(int buffer) =>
        (in Document ctx) =>
        {
            ctx.WriteTo(buffer);

            return "";
        };

    public static string Clear(in Document ctx)
    {
        ctx.Clear();
        return "";
    }

    public static Instruction Dedent(int count) =>
        (in Document ctx) =>
        {
            ctx.Dedent(count);

            return "";
        };

    public static Instruction Get<T>(string name, Func<T, Instruction> bind) =>
        (in Document ctx) => ctx.TryGet<T>(name, out var value) ? bind(value!)(in ctx) : "";

    public static Instruction Get(string name) => (in Document doc) => doc.Stringify(name);

    public static Instruction IfDef(string name, Instruction then, Instruction alt) =>
        (in Document ctx) =>
            ctx.Defines(name) ? then(in ctx) : alt(in ctx);

    public static Instruction IfNDef(string name, Instruction then, Instruction alt) =>
        (in Document ctx) => !ctx.Defines(name) ? then(in ctx) : alt(in ctx);

    public static Instruction Include(params string[] paths) =>
        (in Document doc) =>
        {
            doc.Include(paths);
            return "";
        }

    
        public static Instruction Indent(int count) =>
        (in Document ctx) =>
        {
            ctx.Indent(count);

            return "";
        };

    public static Instruction Read(params int[] buffers) =>
        (in Document ctx) => ctx.Read(buffers);

    public static Instruction ReadLine(params int[] buffers) =>
        (in Document ctx) => ctx.ReadLine(buffers);

    public static Instruction Set<T>(string name, T value) where T : notnull =>
        (in Document ctx) =>
        {
            ctx.Set(name, value);
            return "";
        };

    public static Instruction Unset(string name) =>
        (in Document ctx) =>
        {
            ctx.Unset(name);
            return "";
        };

    public static Instruction Update<T>(string name, T value) where T : notnull =>
        (in Document ctx) =>
        {
            ctx.Update(name, value);
            return "";
        };

    public static Instruction Write(params Instruction[] text) =>
        (in Document ctx) =>
        {
            foreach (var str in text)
                ctx.Write(str(in ctx));

            return "";
        };

    public static Instruction Write(IEnumerable<string> text) =>
        (in Document ctx) =>
        {
            foreach (var str in text)
                ctx.Write(str);

            return "";
        };

    public static Instruction WriteLine(params Instruction[] text) =>
        (in Document ctx) =>
        {
            if (text.Length == 0)
            {
                ctx.Write(Environment.NewLine);
                return "";
            }

            foreach (var str in text)
            {
                ctx.Write(str(in ctx));
                ctx.Write(Environment.NewLine);
            }

            return "";
        };

    public static Instruction WriteLine(IEnumerable<string> text) =>
        (in Document ctx) =>
        {
            ctx.Write(string.Join(Environment.NewLine, text));
            ctx.Write(Environment.NewLine);

            return "";
        };
}
