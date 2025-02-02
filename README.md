# shortid 🆔

[![Build Status](https://travis-ci.org/bolorundurowb/shortid.svg?branch=master)](https://travis-ci.org/bolorundurowb/shortid)  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)  ![NuGet Version](https://img.shields.io/nuget/v/shortid)  [![Coverage Status](https://coveralls.io/repos/github/bolorundurowb/shortid/badge.svg?branch=master)](https://coveralls.io/github/bolorundurowb/shortid?branch=master)

---

## About ShortId 📜

**ShortId** is a lightweight and efficient C# library designed to generate completely random, short, and unique identifiers. These IDs are perfect for use as primary keys, unique identifiers, or any scenario where you need a compact, random string. 🎯

What sets **ShortId** apart is its flexibility—you can specify the length of the IDs (between 8 and 15 characters) and customize the character set. It’s also **thread-safe**, making it ideal for high-performance applications that require generating millions of unique IDs across multiple threads. 💪

---

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
using shortid.Configuration;
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
var options = new GenerationOptions(useNumbers: true);
string id = ShortId.Generate(options);
// Example output: O_bBY-YUkJg
```

#### **Exclude Special Characters**
```csharp
var options = new GenerationOptions(useSpecialCharacters: false);
string id = ShortId.Generate(options);
// Example output: waBfk3z
```

#### **Specify ID Length**
```csharp
var options = new GenerationOptions(length: 9);
string id = ShortId.Generate(options);
// Example output: M-snXzBkj
```

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

## Why Use ShortId? 🌟

- **Flexible Length**: Generate IDs between 8 and 15 characters long.
- **Customizable**: Use your own character set or exclude special characters.
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