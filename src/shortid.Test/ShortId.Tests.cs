using System;
using System.Linq;
using shortid.Utils;
using Shouldly;
using Xunit;

namespace shortid.Test;

public class ShortIdTests
{
    [Fact]
    public void Generate_WithDefaultOptions_ReturnsNonEmptyString()
    {
        var result = ShortId.Generate();

        result.ShouldNotBeNullOrEmpty();
        result.Length.ShouldBeInRange(Constants.MinimumOutputLength, Constants.MaximumAutoLength);
    }

    [Fact]
    public void Generate_WithCustomOptions_ReturnsCorrectLength()
    {
        var options = new ShortIdOptions(length: 10);

        var result = ShortId.Generate(options);

        result.ShouldNotBeNullOrEmpty();
        result.Length.ShouldBe(10);
    }

    [Fact]
    public void Generate_WithNullOptions_ThrowsArgumentNullException()
    {
        ShortIdOptions options = null!;
        Should.Throw<ArgumentNullException>(() => ShortId.Generate(options));
    }

    [Fact]
    public void Generate_WithInvalidLength_ThrowsArgumentException()
    {
        var options = new ShortIdOptions(length: 7);
        Should.Throw<ArgumentException>(() => ShortId.Generate(options));
    }

    [Fact]
    public void Generate_WithSequentialOption_StartsWithTimestampPrefix()
    {
        var options = new ShortIdOptions(generateSequential: true);
        var resultOne = ShortId.Generate(options);
        var resultTwo = ShortId.Generate(options);

        resultOne.ShouldNotBeNullOrEmpty();
        resultTwo.ShouldNotBeNullOrEmpty();

        // the first 5 characters should be the same
        resultOne[..5].ShouldBe(resultTwo[..5]);
    }

    [Fact]
    public void Generate_ShouldNotUseNumbersWhenNotSpecified()
    {
        ShortId.Reset();
        var options = new ShortIdOptions(useNumbers: false, useSpecialCharacters: false, length: 100);
        var response = ShortId.Generate(options);
        response.Any(char.IsNumber).ShouldBeFalse();
    }

    [Fact]
    public void Generate_ShouldNotUseSpecialCharactersWhenNotSpecified()
    {
        ShortId.Reset();
        var options = new ShortIdOptions(useNumbers: false, useSpecialCharacters: false, length: 100);
        var response = ShortId.Generate(options);
        response.Any(c => Constants.Specials.Contains(c)).ShouldBeFalse();
    }

    [Fact]
    public void SetCharacters_WithValidCharacters_UpdatesPool()
    {
        var newChars = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫";

        ShortId.SetCharacters(newChars);
        var result = ShortId.Generate(new ShortIdOptions(length: 10));

        result.ShouldNotBeNullOrEmpty();
        result.Length.ShouldBe(10);
    }

    [Fact]
    public void SetCharacters_WithInvalidCharacters_ThrowsException()
    {
        var invalidChars = string.Empty;
        Should.Throw<ArgumentException>(() => ShortId.SetCharacters(invalidChars));
    }

    [Fact]
    public void SetCharacters_WithTooFewCharacters_ThrowsException()
    {
        var tooFewChars = "ABC";
        Should.Throw<InvalidOperationException>(() => ShortId.SetCharacters(tooFewChars));
    }

    [Fact]
    public void SetCharacters_ShouldRemoveWhitespaceAndDuplicates()
    {
        // char set with only whitespace and duplicate chars, even though it exceeds the minimum length. it should fail validation
        const string charSet =
            " Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ ";

        Should.Throw<InvalidOperationException>(() => { ShortId.SetCharacters(charSet); });
    }

    [Fact]
    public void SetSeed_ChangesRandomOutput()
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
    public void SetSeed_WorksWithValidCharSet()
    {
        const string seed = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫";
        Should.NotThrow(() => { ShortId.SetCharacters(seed); });
    }

    [Fact]
    public void Reset_RestoresDefaultPoolAndRandom()
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