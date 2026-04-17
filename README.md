# ShortID 🆔

[![Build, Test & Coverage](https://github.com/bolorundurowb/shortid/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/bolorundurowb/shortid/actions/workflows/build-and-test.yml) [![codecov](https://codecov.io/gh/bolorundurowb/shortid/graph/badge.svg?token=XBA3CK3YIS)](https://codecov.io/gh/bolorundurowb/shortid) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE) ![NuGet Version](https://img.shields.io/nuget/v/shortid)

ShortId is a small C# library for generating random, URL-friendly identifiers. It targets **.NET Standard 2.0**, so you can use it from modern .NET and .NET Framework projects. Generation is **thread-safe** and tuned for low allocation.

Typical uses include primary keys, opaque tokens, and any place you want a compact random string instead of a GUID.

## Installation

**Package Manager**

```cmd
Install-Package shortid
```

**.NET CLI**

```bash
dotnet add package shortid
```

**PackageReference** (pin the version you want from [NuGet](https://www.nuget.org/packages/shortid/))

```xml
<PackageReference Include="shortid" Version="5.0.0" />
```

## Usage

Add the namespace:

```csharp
using shortid;
```

### Basic generation

Call `ShortId.Generate()` with no arguments to use the default options:

```csharp
string id = ShortId.Generate();
// Example: KXTR_VzGVUoOY
```

Default behaviour (see [Options](#options) for details):

- Length is **15** characters unless you pass a different `length` in `ShortIdOptions`.
- **Special characters** (`_` and `-`) may appear; **numbers are off** by default.
- IDs are **not** sequential unless you opt in.

### Options

Configure generation with `ShortIdOptions`:

| Parameter | Default | Description |
|-----------|---------|-------------|
| `useNumbers` | `false` | Include `0-9` in the character pool. |
| `useSpecialCharacters` | `true` | Include `_` and `-` in the pool. |
| `length` | **15** | Exact output length. Must be **at least 8**. |
| `generateSequential` | `false` | Prefix IDs with a time-based component so lexicographic order follows generation order. |

Examples:

```csharp
// Include digits
var withNumbers = new ShortIdOptions(useNumbers: true);
string id1 = ShortId.Generate(withNumbers);

// No special characters
var noSpecials = new ShortIdOptions(useSpecialCharacters: false);
string id2 = ShortId.Generate(noSpecials);

// Fixed length (minimum 8)
var fixedLength = new ShortIdOptions(length: 9);
string id3 = ShortId.Generate(fixedLength);

// Sequential / monotonic IDs (time prefix + random suffix)
var sequential = new ShortIdOptions(generateSequential: true);
string id4 = ShortId.Generate(sequential);
```

When `generateSequential` is `true`, the first **six** characters encode the current time (centiseconds since the library epoch) in Base85; the rest of the string is random from the active pool. Shorter total lengths leave less room for randomness after the prefix, so prefer **12+** characters for high burst rates.

### Global customisation

**Custom character set**: replace the default pool (letters only before numbers/specials are added per options). The string must contain at least **50** unique non-whitespace characters; duplicates and whitespace are stripped.

```csharp
string characters = "..."; // your alphabet, 50+ unique chars
ShortId.SetCharacters(characters);
```

**Reproducible sequences**: set a fixed seed for the internal `Random` instance (useful for tests; avoid for security-sensitive IDs in production):

```csharp
ShortId.SetSeed(1939048828);
```

**Reset**: restore default character pool and a new unseeded random generator:

```csharp
ShortId.Reset();
```

`SetCharacters`, `SetSeed`, and `Reset` are synchronised and safe to call from multiple threads together with `Generate`.

### Exceptions

- `Generate(null)` throws `ArgumentNullException`.
- `length` below **8** throws `ArgumentException`.
- Invalid `SetCharacters` input throws `ArgumentException` or `InvalidOperationException` as documented in code.

## Benchmarks

Throughput and allocation depend on your CPU, OS, and .NET runtime. Measure on your own hardware using the **BenchmarkDotNet** project in this repository:

```bash
cd src
dotnet run -c Release --project shortid.Benchmarks
```

Optional: append `-- -j short` for a quicker run. Any [BenchmarkDotNet CLI arguments](https://github.com/dotnet/BenchmarkDotNet/blob/master/docs/guide/ConsoleArguments.md) go after `--`.

### Reference run (April 2026)

One complete run on **Windows 11**, **Intel Core Ultra 7 265K**, **.NET 8.0.26**, **BenchmarkDotNet 0.14.0**. Absolute figures will differ on other hardware; relative gaps between configurations tend to hold.

**Length sweep** (`ShortIdLengthBenchmarks`): letters + `_`/`-`, no numbers, varying `length`:

| Length | Mean | Allocated |
|--------|------|-------------|
| 8 | 35.2 ns | 208 B |
| 9 | 39.6 ns | 216 B |
| 10 | 41.9 ns | 224 B |
| 11 | 39.0 ns | 224 B |
| 12 | 40.4 ns | 224 B |
| 13 | 42.0 ns | 232 B |
| 14 | 43.2 ns | 240 B |
| 15 | 44.8 ns | 240 B |

Parameterless `ShortId.Generate()` matches the **length 15** row above (default options: letters, specials on, numbers off).

**Option mix at length 10** (`ShortIdOptionsBenchmarks`; baseline = letters only):

| Configuration | Mean | Ratio vs letters-only |
|---------------|------|------------------------|
| Letters only | 26.8 ns | 1.00 |
| Letters + specials | 35.8 ns | 1.34 |
| Letters + numbers | 36.1 ns | 1.35 |
| Letters + numbers + specials | 60.9 ns | 2.28 |
| Sequential (letters + specials) | 78.8 ns | 2.95 |

**Takeaways:** A minimal character pool and shorter IDs are fastest. Each extra character class adds cost; **sequential** mode is the slowest option tested here but still sub-microsecond per ID on this machine. Allocation scales roughly with string length (about **208–240 B** per ID in the length sweep above, managed heap only).

For **speed**, prefer a short length and `useNumbers: false`, `useSpecialCharacters: false` when you can. For **collision resistance** at high volume, use at least the default **15** characters and consider including digits; sequential IDs help lexicographic ordering but need enough trailing randomness under burst traffic.

## Contributing

Contributions are welcome.

1. **Issues and pull requests**: Open an issue to discuss larger changes; pull requests should stay focused and include tests when behavior changes.
2. **Build**: Open `src/shortid.sln` in your IDE, or from the repo root: `dotnet build src/shortid.sln`.
3. **Tests**: `dotnet test src/shortid.Test/shortid.Test.csproj`
4. **Benchmarks**: See [Benchmarks](#benchmarks); use Release configuration for meaningful results.

Please match existing code style and keep public API changes backward compatible unless there is an agreed major version bump.

## License

ShortId is released under the [MIT License](LICENSE).
