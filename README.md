# `Mackerel`

`Mackerel` is a small functional macro processor library with support for state management and multiple ouput buffers. It can be used to generate, alter, and/or format text in a systematic way.

## Install

Library can be installed via the nuget package manager in Visual Studio or

```
dotnet add package Mackerel --version 1.0.0
```

## Example

```cs
using Mackerel;

using static Mackerel.Macro;

static Instruction HTMLTag(string tag, Macro body) => Block(
    WriteLine($"<{tag}>"),
    Indent(4),
    body,
    Dedent(4),
    NewLine,
    Write($"</{tag}>"));

var example = Block(
    HTMLTag("html",
        Block(
            HTMLTag("head", Text("This will not show up in the ouput.")),
            NewLine,
            HTMLTag("body", 
                HTMLTag("p", Write("This will show up in the output."))
            ))),
    Read()
);

var doc = new Document();

Console.WriteLine(example(in doc));

/* 
<html>
    <head>

    </head>
    <body>
        <p>
            This will show up in the output.
        </p>
    </body>
</html>
*/

```