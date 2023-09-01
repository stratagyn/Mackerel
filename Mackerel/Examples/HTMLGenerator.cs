using static Mackerel.Macro;

namespace Mackerel.Examples;

public static class HTML
{
    private const int INDENTATION = 2;

    public static Instruction ITag(string tag, Instruction body) =>
        Block(Write($"<{tag}>"), body, Write($"</{tag}>"));

    public static Instruction ITag(string tag, string body) =>
        Write($"<{tag}>{body}</{tag}");

    public static Instruction ITag(
        string tag,
        IDictionary<string, string> attributes,
        Instruction body) =>
            Block(
                Write($"<{tag} "),
                Write(
                    Mutate(
                        attributes,
                        attrs => Text(string.Join(
                            ' ',
                            attrs.Select(pair => pair.Value is string str
                                ? $"{pair.Key}=\"{str}\""
                                : $"{pair.Key}={pair.Value}"))))),
                Write(">"),
                body,
                Write($"</{tag}>")
            );

    public static Instruction ITag(
        string tag,
        IDictionary<string, string> attributes,
        string body) =>
            Block(
                Write($"<{tag} "),
                Write(
                    Mutate(
                        attributes,
                        attrs => Text(string.Join(
                            ' ',
                            attrs.Select(pair => pair.Value is string str
                                ? $"{pair.Key}=\"{str}\""
                                : $"{pair.Key}={pair.Value}"))))),
                Write(">"),
                Write(body),
                Write($"</{tag}>")
            );

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

    public static Instruction FTag(
        string tag,
        IDictionary<string, string> attributes,
        Instruction body) =>
            Block(
                Write($"<{tag} "),
                Write(
                    Mutate(
                        attributes,
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

    public static Instruction FTag(
        string tag,
        IDictionary<string, string> attributes,
        string body) =>
            Block(
                Write($"<{tag} "),
                Write(
                    Mutate(
                        attributes,
                        attrs => Text(string.Join(
                            ' ',
                            attrs.Select(pair => pair.Value is string str
                                ? $"{pair.Key}=\"{str}\""
                                : $"{pair.Key}={pair.Value}"))))),
                WriteLine(">"),
                Indent(INDENTATION),
                Write(body),
                Dedent(INDENTATION),
                WriteLine(),
                Write($"</{tag}>")
            );
}
