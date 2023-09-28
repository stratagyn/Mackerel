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
    public static Instruction AppendTo(string path, params int[] buffers) =>
        (in Document doc) =>
        {
            using var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None);
            using var sw = new StreamWriter(fs);

            sw.Write(doc.Read(buffers));
            return "";
        };

    public static Instruction AppendTo(string path, Instruction instr) =>
        (in Document doc) =>
        {
            using var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None);
            using var sw = new StreamWriter(fs);

            sw.Write(instr(in doc));
            return "";
        };

    public static Instruction AppendLineTo(string path, params int[] buffers) =>
        (in Document doc) =>
        {
            using var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None);
            using var sw = new StreamWriter(fs);

            sw.WriteLine(doc.ReadLine(buffers));
            return "";
        };

    public static Instruction AppendLineTo(string path, Instruction instr) =>
        (in Document doc) =>
        {
            using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            using var sw = new StreamWriter(fs);

            sw.WriteLine(instr(in doc));
            return "";
        };

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

    public static Instruction Print(Instruction instr) =>
        (in Document doc) =>
        {
            Console.Write(instr(in doc));
            return "";
        };

    public static Instruction PrintError(Instruction instr) =>
        (in Document doc) =>
        {
            Console.Error.Write(instr(in doc));
            return "";
        };

    public static Instruction PrintLine(Instruction instr) =>
        (in Document doc) =>
        {
            Console.WriteLine(instr(in doc));
            return "";
        };

    public static Instruction PrintErrorLine(Instruction instr) =>
        (in Document doc) =>
        {
            Console.Error.WriteLine(instr(in doc));
            return "";
        };

    public static Instruction SaveTo(string path, params int[] buffers) =>
        (in Document doc) =>
        {
            using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            using var sw = new StreamWriter(fs);

            sw.Write(doc.Read(buffers));
            return "";
        };

    public static Instruction SaveTo(string path, Instruction instr) =>
        (in Document doc) =>
        {
            using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            using var sw = new StreamWriter(fs);

            sw.Write(instr(in doc));
            return "";
        };

    public static Instruction SaveLineTo(string path, params int[] buffers) =>
        (in Document doc) =>
        {
            using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            using var sw = new StreamWriter(fs);

            sw.WriteLine(doc.ReadLine(buffers));
            return "";
        };

    public static Instruction SaveLineTo(string path, Instruction instr) =>
        (in Document doc) =>
        {
            using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            using var sw = new StreamWriter(fs);

            sw.WriteLine(instr(in doc));
            return "";
        };

    public static Instruction RAW(Instruction instr, params int[] buffers) =>
        (in Document doc) =>
        {
            instr(in doc);
            return doc.Read(buffers);
        };

    public static Instruction WAR(params int[] buffers) =>
        (in Document doc) =>
        {
            doc.Write(doc.Read(buffers));
            return "";
        };
}


