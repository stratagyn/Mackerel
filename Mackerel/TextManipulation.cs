using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mackerel;

public static partial class Macro
{
    public static Instruction Concat(params Instruction[] instrs) =>
        (in Document ctx) =>
        {
            var text = new StringBuilder();

            foreach (var instr in instrs)
                text.Append(instr(in ctx));

            return text.ToString();
        };

    public static Instruction Delete(Instruction instr, int startingFrom) =>
        (in Document ctx) =>
        {
            var str = instr(in ctx);

            return startingFrom < 0 || startingFrom >= str.Length
                ? str
                : str.Remove(startingFrom);
        };

    public static Instruction Delete(Instruction instr, int startingFrom, int count) =>
        (in Document ctx) =>
        {
            var str = instr(in ctx);

            return startingFrom < 0 || startingFrom >= str.Length || count <= 0
                ? str
                : str.Remove(startingFrom, Math.Min(str.Length - startingFrom, count));
        };

    public static Instruction Join(string separator, params Instruction[] instrs) =>
        (in Document ctx) =>
        {
            var strs = new string[instrs.Length];

            for (int i = 0; i < instrs.Length; i++)
                strs[i] = instrs[i](in ctx);

            return string.Join(separator, strs);
        };

    public static Instruction Join(Instruction separator, params Instruction[] instrs) =>
        (in Document ctx) =>
        {
            var strs = new StringBuilder();

            for (int i = 0; i < instrs.Length - 1; i++)
            {
                strs.Append(instrs[i](in ctx));
                strs.Append(separator(in ctx));
            }

            if (instrs.Length > 0)
                strs.Append(instrs[^1](in ctx));

            return strs.ToString();
        };

    public static Instruction Lowercase(Instruction instr) =>
        (in Document ctx) => instr(in ctx).ToLower();

    public static Instruction PadEnd(Instruction instr, int count) => PadEnd(instr, " ", count);

    public static Instruction PadEnd(Instruction instr, string padding, int count) =>
        (in Document ctx) =>
        {
            var str = instr(in ctx);

            var padded = new StringBuilder(padding.Length * count + str.Length);

            padded.Append(str);

            for (int i = 0; i < count; i++)
                padded.Append(padding);

            return padded.ToString();
        };

    public static Instruction PadEnd(Instruction instr, Instruction padding, int count) =>
        (in Document ctx) =>
        {
            var str = instr(in ctx);

            var padded = new StringBuilder(count + str.Length);

            padded.Append(str);

            for (int i = 0; i < count; i++)
                padded.Append(padding(in ctx));

            return padded.ToString();
        };

    public static Instruction PadStart(Instruction instr, int count) => PadStart(instr, " ", count);

    public static Instruction PadStart(Instruction instr, string padding, int count) =>
        (in Document ctx) =>
        {
            var str = instr(in ctx);

            if (count <= 0)
                return str;

            var padded = new StringBuilder(padding.Length * count + str.Length);

            for (int i = 0; i < count; i++)
                padded.Append(padding);

            return padded.Append(str).ToString();
        };

    public static Instruction PadStart(Instruction instr, Instruction padding, int count) =>
        (in Document ctx) =>
        {
            var str = instr(in ctx);

            if (count <= 0)
                return str;

            var padded = new StringBuilder(count + str.Length);

            for (int i = 0; i < count; i++)
                padded.Append(padding(in ctx));

            return padded.Append(str).ToString();
        };

    public static Instruction Replace(Instruction input, char pattern, char with) =>
        (in Document ctx) => input(in ctx).Replace(pattern, with);

    public static Instruction Replace(Instruction input, char[] pattern, char[] with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var str = input(in ctx);

            foreach (var c in str)
            {
                var index = Array.IndexOf(pattern, c);

                if (index < 0)
                    expanded.Append(c);
                else if (index < with.Length)
                    expanded.Append(with[index]);
            }

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, string pattern, string with) =>
        (in Document ctx) => input(in ctx).Replace(pattern, with);

    public static Instruction Replace(Instruction input, string pattern, Instruction with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var index = 0;

            while (index < text.Length)
            {
                var next = text.IndexOf(pattern, index);

                if (next < 0)
                {
                    expanded.Append(text[index..]);
                    break;
                }

                if (next > index)
                {
                    expanded.Append(text[index..next]);
                    index = next;
                }

                expanded.Append(with(in ctx));

                index += pattern.Length;
            }

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, string pattern, Func<string, string> with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var index = 0;

            while (index < text.Length)
            {
                var next = text.IndexOf(pattern, index);

                if (next < 0)
                {
                    expanded.Append(text[index..]);
                    break;
                }

                if (next > index)
                {
                    expanded.Append(text[index..next]);
                    index = next;
                }

                expanded.Append(with(pattern));

                index += pattern.Length;
            }

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, string pattern, Func<string, int, string> with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var matched = 0;
            var index = 0;

            while (index < text.Length)
            {
                var next = text.IndexOf(pattern, index);

                if (next < 0)
                {
                    expanded.Append(text[index..]);
                    break;
                }

                if (next > index)
                {
                    expanded.Append(text[index..next]);
                    index = next;
                }

                expanded.Append(with(pattern, matched++));

                index += pattern.Length;
            }

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, Regex pattern, string with) =>
        (in Document ctx) => pattern.Replace(input(in ctx), with);

    public static Instruction Replace(Instruction input, Regex pattern, Instruction with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var match = pattern.Match(text);
            var index = 0;

            while (match.Success)
            {
                if (match.Index > index)
                {
                    expanded.Append(text[index..match.Index]);
                    index = match.Index;
                }

                expanded.Append(with(in ctx));

                index += match.Length;
                match = match.NextMatch();
            }

            if (index < text.Length)
                expanded.Append(text[index..]);

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, Regex pattern, Func<string, string> with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var match = pattern.Match(text);
            var index = 0;

            while (match.Success)
            {
                if (match.Index > index)
                {
                    expanded.Append(text[index..match.Index]);
                    index = match.Index;
                }

                expanded.Append(with(match.Value));

                index += match.Length;
                match = match.NextMatch();
            }

            if (index < text.Length)
                expanded.Append(text[index..]);

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, Regex pattern, Func<string, Instruction> with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var match = pattern.Match(text);
            var index = 0;

            while (match.Success)
            {
                if (match.Index > index)
                {
                    expanded.Append(text[index..match.Index]);
                    index = match.Index;
                }

                expanded.Append(with(match.Value)(in ctx));

                index += match.Length;
                match = match.NextMatch();
            }

            if (index < text.Length)
                expanded.Append(text[index..]);

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, Regex pattern, Func<string, int, string> with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var matched = 0;
            var match = pattern.Match(text);
            var index = 0;

            while (match.Success)
            {
                if (match.Index > index)
                {
                    expanded.Append(text[index..match.Index]);
                    index = match.Index;
                }

                expanded.Append(with(match.Value, matched++));

                index += match.Length;
                match = match.NextMatch();
            }

            if (index < text.Length)
                expanded.Append(text[index..]);

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, Regex pattern, Func<string, int, Instruction> with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var matched = 0;
            var match = pattern.Match(text);
            var index = 0;

            while (match.Success)
            {
                if (match.Index > index)
                {
                    expanded.Append(text[index..match.Index]);
                    index = match.Index;
                }

                expanded.Append(with(match.Value, matched++)(in ctx));

                index += match.Length;
                match = match.NextMatch();
            }

            if (index < text.Length)
                expanded.Append(text[index..]);

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, Instruction pattern, Instruction with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var search = pattern(in ctx);
            var index = 0;

            while (index < text.Length)
            {
                var next = text.IndexOf(search, index);

                if (next < 0)
                {
                    expanded.Append(text[index..]);
                    break;
                }

                if (next > index)
                {
                    expanded.Append(text[index..next]);
                    index = next;
                }

                expanded.Append(with(in ctx));

                index += search.Length;
            }

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, Instruction pattern, Func<string, Instruction> with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var search = pattern(in ctx);
            var index = 0;

            while (index < text.Length)
            {
                var next = text.IndexOf(search, index);

                if (next < 0)
                {
                    expanded.Append(text[index..]);
                    break;
                }

                if (next > index)
                {
                    expanded.Append(text[index..next]);
                    index = next;
                }

                expanded.Append(with(search)(in ctx));

                index += search.Length;
            }

            return expanded.ToString();
        };

    public static Instruction Replace(Instruction input, Instruction pattern, Func<string, int, Instruction> with) =>
        (in Document ctx) =>
        {
            var expanded = new StringBuilder();
            var text = input(in ctx);
            var search = pattern(in ctx);
            var matched = 0;
            var index = 0;

            while (index < text.Length)
            {
                var next = text.IndexOf(search, index);

                if (next < 0)
                {
                    expanded.Append(text[index..]);
                    break;
                }

                if (next > index)
                {
                    expanded.Append(text[index..next]);
                    index = next;
                }

                expanded.Append(with(search, matched++)(in ctx));

                index += search.Length;
            }

            return expanded.ToString();
        };

    public static Instruction Substring(Instruction instr, int from) =>
        (in Document ctx) =>
        {
            var str = instr(in ctx);

            return from < 0 || from >= str.Length
                ? ""
                : str[from..];
        };

    public static Instruction Substring(Instruction instr, int from, int count) =>
        (in Document ctx) =>
        {
            var str = instr(in ctx);

            if (from < 0 || from >= str.Length || count <= 0)
                return "";

            return str[from..Math.Min(from + count, str.Length)];
        };

    public static Instruction Titlecase(Instruction instr) =>
        (in Document ctx) => 
            string.Join(" ", 
                instr(in ctx)
                    .Split(" ")
                    .Select(word => 
                        word.Length == 1
                            ? word.ToUpper()
                            : $"{$"{word[0]}".ToUpper()}{word[1..]}"));

    public static Instruction Trim(Instruction instr, params char[] charset) =>
        (in Document ctx) =>
            charset.Length == 0
                ? instr(in ctx).Trim()
                : charset.Length == 1
                    ? instr(in ctx).Trim(charset[0]) 
                    : instr(in ctx).Trim(charset);

    public static Instruction TrimEnd(Instruction instr, params char[] charset) =>
        (in Document ctx) =>
            charset.Length == 0
                ? instr(in ctx).TrimEnd()
                : charset.Length == 1
                    ? instr(in ctx).TrimEnd(charset[0])
                    : instr(in ctx).TrimEnd(charset);

    public static Instruction TrimStart(Instruction instr, params char[] charset) =>
        (in Document ctx) =>
            charset.Length == 0
                ? instr(in ctx).TrimStart()
                : charset.Length == 1
                    ? instr(in ctx).TrimStart(charset[0])
                    : instr(in ctx).TrimStart(charset);

    public static Instruction Uppercase(Instruction instr) =>
        (in Document ctx) => instr(in ctx).ToUpper();
}
