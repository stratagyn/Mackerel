# `Macro`

Provides methods for managing the state of, as well as, generating, altering, and formatting text within a `Document` instance. Each method is either an `Instruction` or returns an `Instruction`.

An `Instruction` is defined as a function that optionally alters and/or produces text.


```cs
delegate string Instruction(in Document doc)
```
<br>

## Constants

Expands to the empty string.

```cs
string Empty(in Document _)
```

<br>

Replaces all instances of whitespace with, and where several appear reduces them to, a single space character. Returned instruction expands to the squeezed `text`.

```cs
Instruction LongText(string text)
```

<br>

Returned instruction expands to `text`.

```cs
Instruction Text(string text)
```

<br>

Returned instruction expands to the result of `value.ToString()` or the empty string.

```cs
Instruction Str(object value)
```

## Environment

Retrieves the value associated with `name` in a document's environment and converts it to a `string`. If `name` is not defined, nothing happens. The returned instruction expands to the result of calling `ToString` on the associated value or the empty string.

```cs
Instruction Get(string name)
```

Retrieves the value associated with `name` in a document's environment and applies `bind` to it. If `name` is not defined, or is not the given type, nothing happens. The returning instruction expands to the result of `bind` or the empty string.

```cs
Instruction Get<T>(string name, Func<T, Instruction> bind)
```

<br>

Binds `name` to `value` in a document's environment. If `name` is already defined, nothing happens. The returned instruction expands to the empty string.

```cs
Instruction Set<T>(string name, T value)
```

<br>

Unbinds and removes `name` from a document's environment. If `name` is not defined, nothing happens. The returned instruction expands to the empty string.

```cs
Instruction Unset<T>(string name, T value)
```

<br>

Binds `name` to `value` in a document's environment. If `name` is not defined, nothing happens. The returned instruction expands to the empty string.

```cs
Instruction Update<T>(string name, T value)
```

## Transforms

Injects the result of `instruction` into another by applying `bind` to it. The returned instruction expands to the result of the instruction returned from applying `bind` to that of `instruction`.

```cs
Instruction Bind(Instruction instruction, Func<string, Instruction> bind)
```

<br>

Transforms the result of `instruction` by applying `map` to it. The returned instruction expands to the result of applying `map` to that of `instruction`.

```cs
Instruction Map(Instruction instruction, Func<string, Instruction> map)
```

<br>

Injects a value into an instruction using `bind`. When provided with an instruction, the result of the instruction is transformed, into the injected. The returned instruction expands to the result of applying `bind` to the given value or transformed result of `instruction`.

```cs
Instruction Mutate<T>(T value, Func<T, Instruction> bind);
Instruction Mutate<T>(Instruction instruction, Func<string, T> map, Func<T, Instruction> bind);
```

## Conditionals

Applies a `test` to the result of `value`. Returned instruction expands to the result of `then` if the `test` returns `true`, otherwise the result of `alt` is returned.

```cs
Instruction If(Func<string, bool> test, Instruction value, Instruction then, Instruction alt);
Instruction If<T>(Func<T, bool> test, T value, Instruction then, Instruction alt)
```

<br>

Applies a `test` to the result of `value`. Returned instruction expands to the result of `then` if the `test` returns `false`, otherwise the result of `alt` is returned.

```cs
Instruction IfN(Func<string, bool> test, Instruction value, Instruction then, Instruction alt);
Instruction IfN<T>(Func<T, bool> test, T value, Instruction then, Instruction alt)
```

<br>

Checks if `name` is defined in the environment of a document and applies `then`,
if it is, `alt` otherwise.

```cs
Instruction IfDef(string name, Instruction then, Instruction alt)
```

<br>

Checks if `name` is not defined in the environment of a document and applies `then`,
if it is not, `alt` otherwise.

```cs
Instruction IfNDef(string name, Instruction then, Instruction alt)
```

## General

Changes the output buffer of a document to `buffer`. If `buffer` is invalid in the context of a document, subsequent writes to that document are ignored until the buffer is valid again. Returned instruction expands to the empty string.

```cs
Instruction Buffer(int buffer)
```

<br>

Applies each instruction in `instructions` sequentially. The returned instruction expands to the result of the last instruction to be applied.

```cs
Instruction Block(params Instruction[] instructions)
```

<br>

Clears the content of and resets `document`. Expands to the empty string.

```cs
string Clear(in Document ctx)
```

<br>

Writes the concatenated content of each valid file in `paths` to the current buffer of a document. The returned instruction expands to the empty string.

```cs
Instruction Include(params string[] paths);
```

<br>

Concatenates the content of all valid buffers in `buffers`. If no `buffers` is empty, the last written to buffer is used. The returned instruction expands to the concatenated text or empty string. 

```cs
Instruction Read(params int[] buffers);
```

<br>

Concatenates the content of all valid buffers in `buffers` with a new line. The returned instruction expands to the concatenated text or empty string. 

```cs
Instruction ReadLine(params int[] buffers);
```

<br>

Writes the concatenated results of each instruction in `instructions` to the current buffer of a document. The returned instruction expands to the empty string.

```cs
Instruction Write(params Instruction[] instructions);
```

<br>

Writes the concatenated `text` to the current buffer of a document. The returned instruction expands to the empty string.

```cs
Instruction Write(IEnumerable<string> text);
```

<br>

Writes the results of each instruction in `instructions`, concatenated by a new line to the current buffer of a document. The returned instruction expands to the empty string.

```cs
Instruction WriteLine(params Instruction[] instructions);
```

<br>

Writes `text`, concatenated by a new line to the current buffer of a document. The returned instruction expands to the empty string.

```cs
Instruction WriteLine(IEnumerable<string> text);
```

## Formatting

Reduces the indentation of a document by `count` spaces, if possible. If `count` is greater than the indent of the document, the identation is set to `0`. If `count` is invalid, nothing happens. The returned instruction expands to the empty string.

```cs
Instruction Dedent(int count)
```

<br>

Increases the indentation of a document by `count` spaces. If `count` is invalid, nothing happens. The returned instruction expands to the empty string.

```cs
Instruction Indent(int count)
```

## Text

Concatenates the results of each instruction in `instructions`. The returned instruction expands to the concatenated text.

```cs
Instruction Concat(params Instruction[] instructions)
```

<br>

Removes characters from the result of `instruction` starting with the character at `startingFrom` and proceeding until the end of the string. If `count` is provided and valid, only `count` characters are removed. The returned instruction expands to the altered text.

```cs
Instruction Delete(Instruction instruction, int startingFrom);
Instruction Delete(Instruction instruction, int startingFrom, int count);
```

<br>

Concatenates the result of each instruction in `instructions` separated by `separator`. If `separator` is an `Instruction`, each result is separated by subsequent application of `separator`, not the result of a single one. The 

```cs
Instruction Join(string separator, params Instruction[] instructions);
Instruction Join(Instruction separator, params Instruction[] instructions);
```

<br>

Lowercases the result of `instruction`. The returned instruction expands to the lowercased text.

```cs
Instruction Lowercase(Instruction instruction)
```

<br>

Adds `count` instances `padding` to the end of the result `instruction`. If `padding` is not provided, a space character is used. If `padding` is an instance of `Instruction`, the resulting text will be padded by repeated application of the instruction, not a single one. The returned instruction expands to the padded text.

```cs
Instruction PadEnd(Instruction instruction, int count);
Instruction PadEnd(Instruction instruction, string padding, int count);
Instruction PadEnd(Instruction instruction, Instruction padding, int count);
```

<br>

Adds `count` instances `padding` to the start of the result `instruction`. If `padding` is not provided, a space character is used. If `padding` is an instance of `Instruction`, the resulting text will be padded by repeated application of the instruction, not a single one. The returned instruction expands to the padded text.

```cs
Instruction PadStart(Instruction instruction, int count);
Instruction PadStart(Instruction instruction, string padding, int count);
Instruction PadStart(Instruction instruction, Instruction padding, int count);
```

<br>

Systematically replaces text in the result of `instruction`, according to the table below. 

| `pattern`     | `replacement`                           |                                                            |
| :-----------: | :------------------------------: | -------------------------------------------------------- |
| `char`        | `char`                           | Replaces instances of `pattern` with instances of `replacement`. 
| `char[]`      | `char[]`                         | Replaces characters in `pattern` with the character at the same index in `replacement`. If this index does not exist, the character is removed.
| `string`      | `string`                         | Replaces instances of `pattern` with instances of `replacement`.
| `string`      | `Func<string, string>`           | Replaces instances of `pattern` with the result of subsequent applications of `replacement` on the result of each match.
| `string`      | `Func<string, int, string>`      | Replaces instances of `pattern` with the result of subsequent applications of `replacement` on the result of each match, taking the index of each match into consideration.
| `Regex`       | `string`                         | Replaces instances of `pattern` with instances of `replacement`.
| `Regex`       | `Instruction`                    | Replaces instances of `pattern` with the result of subsequent applications of `replacement`.
| `Regex`       | `Func<string, string>`           | Replaces instances of `pattern` with the result of subsequent applications of `replacement` on the result of each match.
| `Regex`       | `Func<string, Instruction>`      | Replaces instances of `pattern` with the result of subsequent applications of `replacement` on the result of each match.
| `Regex`       | `Func<string, int, string>`      | Replaces instances of `pattern` with the result of subsequent applications of `replacement` on the result of each match, taking the index of each match into consideration.
| `Regex`       | `Func<string, int, Instruction>` | Replaces instances of `pattern` with the result of subsequent applications of `replacement` on the result of each match, taking the index of each match into consideration.
| `Instruction` | `Instruction`                    | Replaces instances of `pattern` with the result of subsequent applications of `replacement`.
| `Instruction` | `Func<string, Instruction>`      | Replaces instances of `pattern` with the result of subsequent applications of `replacement` on the result of each match.
| `Instruction` | `Func<string, int, Instruction>` | Replaces instances of `pattern` with the result of subsequent applications of `replacement` on the result of each match, taking the index of each match into consideration.


```cs
Instruction Replace(Instruction input, char pattern, char replacement);
Instruction Replace(Instruction input, string pattern, string replacement);
Instruction Replace(Instruction input, string pattern, Func<string, string> replacement);
Instruction Replace(Instruction input, string pattern, Func<string, int, string> replacement);
Instruction Replace(Instruction input, Regex pattern, string replacement);
Instruction Replace(Instruction input, Regex pattern, Instruction replacement);
Instruction Replace(Instruction input, Regex pattern, Func<string, string> replacement);
Instruction Replace(Instruction input, Regex pattern, Func<string, Instruction> replacement);
Instruction Replace(Instruction input, Regex pattern, Func<string, int, string> replacement);
Instruction Replace(Instruction input, Regex pattern, Func<string, int, Instruction> replacement);
Instruction Replace(Instruction input, Instruction pattern, Instruction replacement);
Instruction Replace(Instruction input, Instruction pattern, Func<string, Instruction> replacement);
Instruction Replace(Instruction input, Instruction pattern, Func<string, int, Instruction> replacement);
```

<br>

Collects the characters from the result of `instruction` starting with the character at `startingFrom` and proceeding until the end of the string. If `count` is provided and valid, only `count` characters are collected. The returned instruction expands to the substring text.

```cs
Instruction Substring(Instruction instruction, int startingFrom);
Instruction Substring(Instruction instruction, int startingFrom, int count);
```

<br>

Titlecases the result of `instruction`.

```cs
Instruction Titlecase(Instruction instruction)
```

<br>

Removes characters from either end of the result of `instruction` until a character is found not in `charset`. If `charset` is empty, whitespace is removed from both ends. The returned instruction expands to the altered text.

```cs
Instruction Trim(Instruction instruction, params char[] charset)
```

<br>

Removes characters from the end of the result of `instruction` until a character is found not in `charset`. If `charset` is empty, whitespace is removed from both ends. The returned instruction expands to the altered text.

```cs
Instruction TrimEnd(Instruction instruction, params char[] charset)
```

<br>

Removes characters from the start of the result of `instruction` until a character is found not in `charset`. If `charset` is empty, whitespace is removed from both ends. The returned instruction expands to the altered text.

```cs
Instruction TrimStart(Instruction instruction, params char[] charset)
```

<br>

Capitalizes the result of `instruction`.

```cs
Instruction Uppercase(Instruction instruction)
```