# `Mackerel`

![](https://img.shields.io/badge/passing-true-green)
![](https://img.shields.io/badge/coverage-96.5%25-green)

`Mackerel` is a small functional macro processor library with support for state management and multiple output buffers. It can be used to generate, alter, and/or format text in a systematic way.

## Install

Library can be installed via the nuget package manager in Visual Studio or

```
dotnet add package Mackerel --version 1.0.0
```

## Documentation

Refer to articles in [docs](https://github.com/stratagyn/Mackerel/tree/master/docs) for a full list of predefined instructions.

## Example: HTMLGenerator

```cs
using Mackerel;

using static Mackerel.Macro;

using Attrs = IDictionary<string, object>;

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
        ITag(tag, attrs, Write(body))

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
```

<br>

Reproducing this snippet generated with the help of https://lotremipsum.com/result-advanced.php

<br>

```html
<!DOCTYPE html>
<html>
  <head>
    <title>Mackerel: HTMLGenerator</title>
  </head>
  <body style="width: 100%">
    <h1>Gravy piety Nazgûl knocking what about?</h1>
    <p>
        Poisoned observation lsengard parted stubbornness may golf Thranduil. All right, then. Keep your secrets. Goblins adjusted diversion form loses darkness.
    </p>
    <h2>Chest clot-head's deserted World secrets poring?</h2>
    <p>
      A wizard is never late, Frodo Baggins. Nor is he early. He arrives precisely when he means to. Halflings heathen surprises mattered spawn metal stupidity! Sorceress ceases tilled Samwise delays abyss walks amaze remarked 17?
    </p>
    <h3>Hill legions inspection Haldir solitary yearns bitterness.</h3>
    <p>
      Thofin love leaderless flee t riding women. Nobody tosses a Dwarf. Mouthful web fishes smoked bowl appearances hmm Mereth guessed licensed. Lessened Esgaroth paws
    </p>
    <ul>
      <li>Sauron the White?</li>
      <li>Misty Mountain.</li>
      <li>Ringwraiths.</li>
      <li>Caradhras.</li>
      <li>Gollum.</li>
    </ul>
  </body>
</html>
```

<br>

We get

<br>

```cs
var lorem = Block(
    WriteLine("<!DOCTYPE html>"),
    HTML.FTag("html",
        Block(
            HTML.FTag("head",
                HTML.ITag("title", "Mackerel: HTMLGenerator")
            ),
            WriteLine(),
            HTML.FTag("body", 
                new Dictionary<string, object>() { ["style"] = "width: 100%;"}
                Join(
                    Environment.NewLine,
                    HTML.ITag("h1", "Gravy piety Nazgûl knocking what about?"),
                    HTML.FTag("p", Write(LongText(
                        """
                            Poisoned observation lsengard parted stubbornness 
                            may golf Thranduil. All right, then. Keep your 
                            secrets. Goblins adjusted diversion form loses darkness.
                        """)
                    )),
                    HTML.ITag("h2", "Chest clot-head's deserted World secrets poring?"),
                    HTML.FTag("p", Write(LongText(
                        """
                            A wizard is never late, Frodo Baggins. Nor is he 
                            early. He arrives precisely when he means to. 
                            Halflings heathen surprises mattered spawn metal 
                            stupidity! Sorceress ceases tilled Samwise delays  
                            abyss walks amaze remarked 17?"
                        """)
                    )),
                    HTML.ITag("h2", "Hill legions inspection Haldir solitary yearns bitterness."),
                    HTML.FTag("p", Write(LongText(
                        """
                            Thofin love leaderless flee t riding women. Nobody 
                            tosses a Dwarf. Mouthful web fishes smoked bowl 
                            appearances hmm Mereth guessed licensed. Lessened 
                            Esgaroth paws.
                        """)
                    )),
                    HTML.FTag("ul",
                        Join(
                            Environment.NewLine,
                            HTML.ITag("li", "Sauron the White?")
                            HTML.ITag("li", "Misty Mountain."),
                            HTML.ITag("li", "Ringwraiths."),
                            HTML.ITag("li", "Caradhras."),
                            HTML.ITag("li", "Gollum.")
                        )
                    )
                )
            )
        )
    ),
    Read()
)

var doc = new Document();

Console.WriteLine(lorem(in doc));
```

This will output the above HTML.