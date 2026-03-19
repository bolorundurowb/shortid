# ShortID 🆔

[![Build, Test & Coverage](https://github.com/bolorundurowb/shortid/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/bolorundurowb/shortid/actions/workflows/build-and-test.yml) [![codecov](https://codecov.io/gh/bolorundurowb/shortid/graph/badge.svg?token=XBA3CK3YIS)](https://codecov.io/gh/bolorundurowb/shortid)  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)  ![NuGet Version](https://img.shields.io/nuget/v/shortid) 


**ShortId** is a lightweight and efficient C# library designed to generate completely random, short, and unique identifiers. These IDs are perfect for use as primary keys, unique identifiers, or any scenario where you need a compact, random string.

What sets **ShortId** apart is its flexibility, you can specify the length of the IDs (minimum of 8 characters) and customise the character set. It’s also **thread-safe**, making it ideal for high-performance applications that require generating millions of unique IDs across multiple threads.


## Getting Started 🚀

### Installation

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


### Customize ID Generation

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

You can measure throughput on your own machine using the **BenchmarkDotNet** project included in this repository:

```bash
cd src
dotnet run -c Release --project shortid.Benchmarks
```

*(Optional quick run: append `-- -j short`. Pass any [BenchmarkDotNet CLI arguments](https://github.com/dotnet/BenchmarkDotNet/blob/master/docs/guide/ConsoleArguments.md) after `--`.)*

Absolute timings will vary depending on your CPU, OS, and .NET version. The findings below were recorded on an Intel Core Ultra 7 265K running .NET 8.

### Key Performance Findings

The library is extremely fast and allocates very little memory (just over 200 bytes per ID), but your configuration choices do impact generation speed:

* **Simplicity is the fastest:** Generating IDs using **only letters** is the absolute fastest method (~25 nanoseconds).
* **Complexity adds minor overhead:** Adding numbers or special characters makes generation roughly 35% to 70% slower than using letters alone.
* **Length barely impacts speed:** Increasing your ID length from 8 characters to 15 characters only adds about 12 nanoseconds of execution time.
* **Sequential mode is heavier:** Generating time-based sequential IDs takes the most time (~75 nanoseconds), which is about 3x slower than a standard random letter ID.

### Choosing Your Settings: Speed vs. Uniqueness

When configuring ShortId, you are balancing raw generation speed against the risk of generating the same ID twice (a collision).

**For maximum performance (pure speed):**
Turn off numbers and special characters (`useNumbers: false`, `useSpecialCharacters: false`) and stick to a shorter length (e.g., 8). This is perfect for low-volume applications.

**For high-volume applications (millions of IDs):**
Do not sacrifice uniqueness for speed. Increase your ID length to **10–14 characters** and include numbers. The performance cost is only a few extra nanoseconds, but it drastically reduces the chance of collisions.

**When using Sequential IDs (`generateSequential: true`):**
Sequential IDs are brilliant for database indexing because the first few characters are based on the current time. However, because the time component takes up space, fewer characters are left for randomness. **Avoid short sequential IDs (e.g., length 8) if you expect massive traffic bursts**, as generating thousands of IDs in the exact same fraction of a second increases collision risk. Prefer longer lengths (12+) for heavy burst traffic.

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

Whether you need unique IDs for database keys, URLs, or any other purpose, **ShortId** has you covered. Install the package, follow the examples, and start generating unique IDs in seconds!

**Happy Coding!**
