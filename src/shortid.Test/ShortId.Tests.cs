using System;
using System.Linq;
using shortid.Utils;
using OmniAssert;
using Xunit;

namespace shortid.Test;

public class ShortIdTests
{
    [Fact]
    public void Generate_WithDefaultParameters_ReturnsStringOfDefaultLength()
    {
        var result = ShortId.Generate();

        result.Verify().NotToBeNull();
        result.Verify().NotToBeEmpty();
        result.Length.Verify().ToBe(Constants.DefaultOutputLength);
    }

    [Fact]
    public void Generate_WithExplicitLength_ReturnsMatchingLength()
    {
        var options = new ShortIdOptions(length: 10);

        var result = ShortId.Generate(options);

        result.Verify().NotToBeNull();
        result.Verify().NotToBeEmpty();
        result.Length.Verify().ToBe(10);
    }

    [Fact]
    public void Generate_WithNullOptions_ThrowsArgumentNullException()
    {
        ShortIdOptions options = null!;
        Action act = () => ShortId.Generate(options);
        act.Throws<ArgumentNullException>();
    }

    [Fact]
    public void Generate_WithLengthBelowMinimum_ThrowsArgumentException()
    {
        var options = new ShortIdOptions(length: 7);
        Action act = () => ShortId.Generate(options);
        act.Throws<ArgumentException>();
    }

    [Fact]
    public void Generate_WithSequentialEnabled_SharesTimestampPrefixAcrossCalls()
    {
        var options = new ShortIdOptions(generateSequential: true);
        var resultOne = ShortId.Generate(options);
        var resultTwo = ShortId.Generate(options);

        resultOne.Verify().NotToBeNull();
        resultOne.Verify().NotToBeEmpty();
        resultTwo.Verify().NotToBeNull();
        resultTwo.Verify().NotToBeEmpty();

        // the first 6 characters should be the same
        resultOne[..6].Verify().ToBe(resultTwo[..6]);
    }

    [Fact]
    public void Generate_WithNumbersDisabled_ExcludesDigits()
    {
        ShortId.Reset();
        var options = new ShortIdOptions(useNumbers: false, useSpecialCharacters: false, length: 100);
        var response = ShortId.Generate(options);
        response.Any(char.IsNumber).Verify().ToBeFalse();
    }

    [Fact]
    public void Generate_WithSpecialCharactersDisabled_ExcludesSpecialCharacters()
    {
        ShortId.Reset();
        var options = new ShortIdOptions(useNumbers: false, useSpecialCharacters: false, length: 100);
        var response = ShortId.Generate(options);
        response.Any(c => Constants.Specials.Contains(c)).Verify().ToBeFalse();
    }

    [Fact]
    public void SetCharacters_WithValidPool_GeneratesIdsFromCustomAlphabet()
    {
        var newChars = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫";

        ShortId.SetCharacters(newChars);
        var result = ShortId.Generate(new ShortIdOptions(length: 10));

        result.Verify().NotToBeNull();
        result.Verify().NotToBeEmpty();
        result.Length.Verify().ToBe(10);
    }

    [Fact]
    public void SetCharacters_WithEmptyString_ThrowsArgumentException()
    {
        var invalidChars = string.Empty;
        Action act = () => ShortId.SetCharacters(invalidChars);
        act.Throws<ArgumentException>();
    }

    [Fact]
    public void SetCharacters_WithTooFewUniqueCharacters_ThrowsInvalidOperationException()
    {
        var tooFewChars = "ABC";
        Action act = () => ShortId.SetCharacters(tooFewChars);
        act.Throws<InvalidOperationException>();
    }

    [Fact]
    public void SetCharacters_WhenDedupedLengthBelowMinimum_ThrowsInvalidOperationException()
    {
        // char set with only whitespace and duplicate chars, even though it exceeds the minimum length. it should fail validation
        const string charSet =
            " Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ  Ⓐ  ⓛ ⓜ ⑩⑫ Ⓐ ";

        Action act = () => ShortId.SetCharacters(charSet);
        act.Throws<InvalidOperationException>();
    }

    [Fact]
    public void SetSeed_WithSameSeedRepeated_ProducesIdenticalIds()
    {
        var seed = 12345;
        var options = new ShortIdOptions(length: 10);

        ShortId.SetSeed(seed);
        var resultOne = ShortId.Generate(options);
        ShortId.SetSeed(seed);
        var resultTwo = ShortId.Generate(options);

        // results should be the same
        resultOne.Verify().ToBe(resultTwo);
    }

    [Fact]
    public void SetCharacters_WithSufficientUniqueCharacters_DoesNotThrow()
    {
        const string characters =
            "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫";
        Action act = () => ShortId.SetCharacters(characters);
        act.NotThrow();
    }

    [Fact]
    public void Reset_AfterCustomPoolAndSeed_RevertsToDefaultGeneration()
    {
        const string seed = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫";
        ShortId.SetCharacters(seed);
        ShortId.SetSeed(12345);
        var options = new ShortIdOptions(length: 10);

        ShortId.Reset();
        var result = ShortId.Generate(options);

        result.Verify().NotToBeNull();
        result.Verify().NotToBeEmpty();
        result.Length.Verify().ToBe(10);
    }
}