# `Document`

The `Document` class is a stateful object managing multiple output buffers. 
Methods in the `Macro` class are used to mutate and access the state of an 
instance.

## Construction

Initializes a new `Document` instance with a predefined `environment` and 
`bufferCount` output buffers. If `bufferCount` is less than `1`, the default 
`10` is used.

```cs
Document(IEnumerable<KeyValuePair<string, object>> environment, int bufferCount = 10)
```

<br>

Initializes a new `Document` instance with `bufferCount` output buffers. If 
`bufferCount` is less than `1`, the default `10` is used.

```cs
Document(int bufferCount = 10)
```
