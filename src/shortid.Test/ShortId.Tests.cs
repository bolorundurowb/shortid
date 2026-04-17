using System;
using System.Linq;
using shortid.Utils;
using Shouldly;
using Xunit;

namespace shortid.Test;

public class ShortIdTests
{
    [Fact]
    public void Generate_DefaultOptions_ShouldReturnNonEmptyString()
    {
        var result = ShortId.Generate();

        result.ShouldNotBeNullOrEmpty();
        result.Length.ShouldBe(Constants.DefaultOutputLength);
    }

    [Fact]
    public void Generate_CustomOptions_ShouldReturnCorrectLength()
    {
        var options = new ShortIdOptions(length: 10);

        var result = ShortId.Generate(options);

        result.ShouldNotBeNullOrEmpty();
        result.Length.ShouldBe(10);
    }

    [Fact]
    public void Generate_NullOptions_ShouldThrowArgumentNullException()
    {
        ShortIdOptions options = null!;
        Should.Throw<ArgumentNullException>(() => ShortId.Generate(options));
    }

    [Fact]
    public void Generate_InvalidLength_ShouldThrowArgumentException()
    {
        var options = new ShortIdOptions(length: 7);
        Should.Throw<ArgumentException>(() => ShortId.Generate(options));
    }

    [Fact]
    public void Generate_SequentialOption_ShouldStartWithTimestampPrefix()
    {
        var options = new ShortIdOptions(generateSequential: true);
        var resultOne = ShortId.Generate(options);
        var resultTwo = ShortId.Generate(options);

        resultOne.ShouldNotBeNullOrEmpty();
        resultTwo.ShouldNotBeNullOrEmpty();

        // the first 6 characters should be the same
        resultOne[..6].ShouldBe(resultTwo[..6]);
    }

    [Fact]
    public void Generate_NoNumbersOption_ShouldExcludeNumbers()
    {
        ShortId.Reset();
        var options = new ShortIdOptions(useNumbers: false, useSpecialCharacters: false, length: 100);
        var response = ShortId.Generate(options);
        response.Any(char.IsNumber).ShouldBeFalse();
    }

    [Fact]
    public void Generate_NoSpecialCharactersOption_ShouldExcludeSpecialCharacters()
    {
        ShortId.Reset();
        var options = new ShortIdOptions(useNumbers: false, useSpecialCharacters: false, length: 100);
        var response = ShortId.Generate(options);
        response.Any(c => Constants.Specials.Contains(c)).ShouldBeFalse();
    }

    [Fact]
    public void SetCharacters_ValidCharacters_ShouldUpdatePool()
    {
        var newChars = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫";

        ShortId.SetCharacters(newChars);
        var result = ShortId.Generate(new ShortIdOptions(length: 10));

        result.ShouldNotBeNullOrEmpty();
        result.Length.ShouldBe(10);
    }

    [Fact]
    public void SetCharacters_InvalidCharacters_ShouldThrowException()
    {
        var invalidChars = string.Empty;
        Should.Throw<ArgumentException>(() => ShortId.SetCharacters(invalidChars));
    }

    [Fact]
    public void SetCharacters_TooFewCharacters_ShouldThrowException()
    {
        var tooFewChars = "ABC";
        Should.Throw<InvalidOperationException>(() => ShortId.SetCharacters(tooFewChars));
    }

    [Fact]
    public void SetCharacters_WhitespaceAndDuplicates_ShouldThrowException()
    {
        // char set with only whitespace and duplicate chars, even though it exceeds the minimum length. it should fail validation
        const string charSet =
            " Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ ";

        Should.Throw<InvalidOperationException>(() => { ShortId.SetCharacters(charSet); });
    }

    [Fact]
    public void SetSeed_ShouldChangeRandomOutput()
    {
        var seed = 12345;
        var options = new ShortIdOptions(length: 10);

        ShortId.SetSeed(seed);
        var resultOne = ShortId.Generate(options);
        ShortId.SetSeed(seed);
        var resultTwo = ShortId.Generate(options);

        // results should be the same
        resultOne.ShouldBe(resultTwo);
    }

    [Fact]
    public void SetSeed_ValidCharSet_ShouldNotThrow()
    {
        const string seed = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫";
        Should.NotThrow(() => { ShortId.SetCharacters(seed); });
    }

    [Fact]
    public void Reset_ShouldRestoreDefaultPoolAndRandom()
    {
        const string seed = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫";
        ShortId.SetCharacters(seed);
        ShortId.SetSeed(12345);
        var options = new ShortIdOptions(length: 10);

        ShortId.Reset();
        var result = ShortId.Generate(options);

        result.ShouldNotBeNullOrEmpty();
        result.Length.ShouldBe(10);
    }
}