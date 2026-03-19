# ShortID 🆔

[![Build, Test & Coverage](https://github.com/bolorundurowb/shortid/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/bolorundurowb/shortid/actions/workflows/build-and-test.yml) [![codecov](https://codecov.io/gh/bolorundurowb/shortid/graph/badge.svg?token=XBA3CK3YIS)](https://codecov.io/gh/bolorundurowb/shortid)  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)  ![NuGet Version](https://img.shields.io/nuget/v/shortid) 


**ShortId** is a lightweight and efficient C# library designed to generate completely random, short, and unique identifiers. These IDs are perfect for use as primary keys, unique identifiers, or any scenario where you need a compact, random string. 🎯

What sets **ShortId** apart is its flexibility—you can specify the length of the IDs (between 8 and 15 characters) and customize the character set. It’s also **thread-safe**, making it ideal for high-performance applications that require generating millions of unique IDs across multiple threads. 💪


## Getting Started 🚀

### Installation 📦

You can add **ShortId** to your project using one of the following methods:

#### **Package Manager**
```cmd
Install-Package shortid
```

#### **.NET CLI**
```bash
dotnet add package shortid
```

#### **PackageReference**
```xml
<PackageReference Include="shortid" />
```

---

## Usage 🛠️

### Add the Namespace
First, include the **ShortId** namespace in your code:
```csharp
using shortid;
```

### Generate a Random ID
To generate a random ID of default length (between 8 and 15 characters), simply call the `Generate` method:
```csharp
string id = ShortId.Generate();
// Example output: KXTR_VzGVUoOY
```

---

### Customize ID Generation 🎨

**ShortId** provides several options to tailor the generated IDs to your needs:

#### **Include Numbers**
```csharp
var options = new ShortIdOptions(useNumbers: true);
string id = ShortId.Generate(options);
// Example output: O_bBY-YUkJg
```

#### **Exclude Special Characters**
```csharp
var options = new ShortIdOptions(useSpecialCharacters: false);
string id = ShortId.Generate(options);
// Example output: waBfk3z
```

#### **Specify ID Length**
```csharp
var options = new ShortIdOptions(length: 9);
string id = ShortId.Generate(options);
// Example output: M-snXzBkj
```

#### **Generate Sequential IDs**
For scenarios where you need IDs to be monotonic (i.e., each ID is greater than the previous one), you can enable sequential ID generation:
```csharp
var options = new ShortIdOptions(generateSequential: true);
string id = ShortId.Generate(options);
// Example output: 00000r_v
```
When this option is enabled, the first 6 characters of the ID represent a Base85 encoded timestamp, followed by random characters to complete the requested length.

---

## Customize ShortId 🎛️

**ShortId** allows you to fully customize the character set and even seed the random number generator for reproducible results.

### Change the Character Set
You can define your own character set for ID generation:
```csharp
string characters = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫"; // Custom character set
ShortId.SetCharacters(characters);
```

**Note:**
- The character set must contain at least **50 unique characters**.
- Duplicate and whitespace characters are automatically removed.

---

### Set a Random Seed
For reproducible results, you can set a seed for the random number generator:
```csharp
int seed = 1939048828;
ShortId.SetSeed(seed);
```

---

### Reset Customizations
To reset all customizations (character set, seed, etc.) to their defaults, use the `Reset` method:
```csharp
ShortId.Reset();
```

---

## Benchmarks

Measure throughput on your machine with the **BenchmarkDotNet** project in this repo:

```bash
cd src
dotnet run -c Release --project shortid.Benchmarks
```

Optional quick run: append `-- -j short`. Pass any [BenchmarkDotNet CLI arguments](https://github.com/dotnet/BenchmarkDotNet/blob/master/docs/guide/ConsoleArguments.md) after `--`.

A sample output snapshot is kept in [`benchmarks/SampleResults.md`](benchmarks/SampleResults.md). Absolute timings vary by CPU, OS, and .NET version—compare configurations on the **same** machine.

---

## Choosing length: performance vs collisions

**Performance:** Fastest generation uses the **shortest** length you can accept and the **smallest** pool—`useNumbers: false` and `useSpecialCharacters: false` (49 letters only). Each extra character and larger alphabet adds a small cost (see benchmarks).

**Random IDs** (default `generateSequential: false`): treat each ID as uniform over \(N^L\) possibilities, where \(L\) is length and \(N\) is the alphabet size:

| Options | Approx. \(N\) |
|---------|---------------|
| Letters only | 49 |
| + `_` `-` (default-style) | 51 |
| + digits, no specials | 59 |
| Digits + specials | 61 |

For \(M\) IDs drawn independently, the usual birthday approximation for a collision is:

\[
P(\text{collision}) \approx 1 - \exp\left(-\frac{M(M-1)}{2 N^L}\right)
\]

Solve for minimum \(L\) by requiring \(N^L \gg M^2\) (e.g. \(\frac{M^2}{2N^L} &lt; 10^{-3}\) for rough “&lt;0.1%” scale).

**Rule of thumb (random mode, \(N \approx 51\)):**

| Expected IDs \(M\) | Suggested \(L\) (order of magnitude) |
|-------------------:|-------------------------------------|
| \(10^4\) | 8 is usually fine |
| \(10^6\) | 9–10 |
| \(10^9\) | 13+ |

If you use **only letters** (\(N=49\)), add about one extra character vs the table above for similar risk.

**Sequential IDs** (`generateSequential: true`): the first **6** characters are a time component; only the remaining \(L - 6\) characters are random **per centisecond**. At \(L = 8\) you only have **2** random symbols per time bucket—high collision risk if you generate many IDs in the same centisecond. Prefer **longer** IDs or non-sequential mode for burst traffic.

---

## Why Use ShortId? 🌟

- **Flexible Length**: Generate IDs from 8 characters long to whatever length you need.
- **Customizable**: Use your own character set or exclude certain characters.
- **Thread-Safe**: Perfect for multi-threaded applications.
- **Lightweight**: Minimal overhead, maximum performance.
- **Easy to Use**: Simple API with just a few methods.

---

## License 📜

**ShortId** is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

---

## Get Started Today! 🎉

Whether you need unique IDs for database keys, URLs, or any other purpose, **ShortId** has you covered. Install the package, follow the examples, and start generating unique IDs in seconds! ⏱️

**Happy Coding!** 🚀
