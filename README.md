# shortid

[![Build Status](https://travis-ci.org/bolorundurowb/shortid.svg?branch=master)](https://travis-ci.org/bolorundurowb/shortid)  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE) [![NuGet Badge](https://buildstats.info/nuget/shortid)](https://www.nuget.org/packages/shortid) [![Coverage Status](https://coveralls.io/repos/github/bolorundurowb/shortid/badge.svg?branch=master)](https://coveralls.io/github/bolorundurowb/shortid?branch=master)

## About ShortId

A CSharp library to generate completely random short id's. They can be used as primary keys or unique identifiers. This library is different in that you can specify the length of the ids to be generated. This library is thread-safe and can generate millions of unique ids across multiple threads.

## Getting Started

To make use of the `shortid`, add it to your project via the Nuget package manager UI or console via this command:

#### Package Manager

```
Install-Package shortid
```

#### .NET CLI
```
> dotnet add package shortid
```

#### PackageReference
```csharp
<PackageReference Include="shortid" />
```

## Usage

Add the following using command to the top of your csharp code file:

```csharp
using shortid;
using shortid.Configuration;
```

This gives your code access the classes and methods of the `shortid` namespace.

To generate a unique id of any length between 8 and 15, you call the `Generate` method without parameters.

```csharp
string id = ShortId.Generate();
// id = KXTR_VzGVUoOY
```

If you want to include numbers in the generated id, then you call the `Generate` method with options indicating your preference.

```csharp
var options = new GenerationOptions
{
  UseNumbers = true
};
string id = ShortId.Generate(options);
// id = O_bBY-YUkJg
```

If you do not want special characters *i.e _ and -* in your generated id, then call the `Generate` method with options indicating your preferences.

```csharp
var options = new GenerationOptions
{
  UseSpecialCharacters = false
};
string id = ShortId.Generate(options);
// id = waBfk3z
```

If you want to specify the length of the generated id, call the `Generate` method with options indicating your preferences.

```csharp
var options = new GenerationOptions
{
  Length = 9
};
string id = ShortId.Generate(options);
// id = M-snXzBkj
```


## Customize ShortId

`ShortId` has several features that help with customizing the ids generated. Characters sets can be introduced and the random number generator can be seeded.

To change the character set in use, run the following:

```csharp
string characters = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫"; //whatever you want;
ShortId.SetCharacters(characters);
```

**NOTE: the new character set must not be `null`, an empty string or whitespace. Also, all whitespace and duplicate characters would be removed, finally the character set cannot be less than 50 characters.**

`ShortId` also allows the seed for the random number generator to be set.

To set the seed, run the following:

```csharp
int seed = 1939048828;
ShortId.SetSeed(seed);
```

Finally, `ShortId` allows for all customizations to be reset using the following:

```csharp
ShortId.Reset();
```