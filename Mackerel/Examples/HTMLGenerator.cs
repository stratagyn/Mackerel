using static Mackerel.Macro;

namespace Mackerel.Examples;

using Attrs = IDictionary<string, string>;

public static class HTML
{
   private const int INDENTATION = 2;

   public static Instruction ITag(string tag, Instruction body) =>
       Block(Write($"<{tag}>"), body, Write($"</{tag}>"));

   public static Instruction ITag(string tag, string body) =>
       Write($"<{tag}>{body}</{tag}");

   public static Instruction ITag(string tag, Attrs attrs, Instruction body) =>
       Block(
           Write($"<{tag} "),
           Write(
               Mutate(
                   attrs,
                   attrs => Text(string.Join(
                       ' ',
                       attrs.Select(pair => pair.Value is string str
                           ? $"{pair.Key}=\"{str}\""
                           : $"{pair.Key}={pair.Value}"))))),
           Write(">"),
           body,
           Write($"</{tag}>")
       );

   public static Instruction ITag(string tag, Attrs attrs, string body) =>
       ITag(tag, attrs, Write(body));

   public static Instruction FTag(string tag, Instruction body) =>
       Block(
           WriteLine($"<{tag}>"),
           Indent(INDENTATION),
           body,
           Dedent(INDENTATION),
           WriteLine(),
           Write($"</{tag}>")
       );

   public static Instruction FTag(string tag, string body) =>
       Block(
           WriteLine($"<{tag}>"),
           Indent(INDENTATION),
           Write(body),
           Dedent(INDENTATION),
           WriteLine(),
           Write($"</{tag}>")
       );

   public static Instruction FTag(string tag, Attrs attrs, Instruction body) =>
       Block(
           Write($"<{tag} "),
           Write(
               Mutate(
                   attrs,
                   attrs => Text(string.Join(
                       ' ',
                       attrs.Select(pair => pair.Value is string str
                           ? $"{pair.Key}=\"{str}\""
                           : $"{pair.Key}={pair.Value}"))))),
           WriteLine(">"),
           Indent(INDENTATION),
           body,
           Dedent(INDENTATION),
           WriteLine(),
           Write($"</{tag}>")
       );

   public static Instruction FTag(string tag, Attrs attrs, string body) =>
       FTag(tag, attrs, Write(body));
}