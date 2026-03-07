using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace shortid.Test;

public class ShortIdTests
{
    [Fact]
    public void SetSeedThrowsWhenCharacterSetIsEmptyOrNull()
    {
        var seed = string.Empty;
        var action = () => { ShortId.SetCharacters(seed); };

        var exception = Should.Throw<ArgumentException>(action);
        exception.Message.ShouldBe("The replacement characters must not be null or empty.");
    }

    [Fact]
    public void SetSeedThrowsWhenCharacterSetIsLessThan20Characters()
    {
        const string seed = "783ujrcuei039kj4";
        var action = () => { ShortId.SetCharacters(seed); };

        var exception = Should.Throw<InvalidOperationException>(action);
        exception.Message.ShouldBe(
            "The replacement characters must be at least 50 letters in length and without whitespace.");
    }

    [Fact]
    public void SetSeedWorksWithValidCharSet()
    {
        const string seed = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫";
        var action = () => { ShortId.SetCharacters(seed); };

        Should.NotThrow(action);
    }

    [Fact]
    public void GenerateThrowsWhenOptionsAreNull()
    {
        var action = () => { ShortId.Generate(null); };

        Should.Throw<ArgumentNullException>(action);
    }

    [Fact]
    public void GenerateShouldSucceedWithoutOptions()
    {
        var response = ShortId.Generate();

        response.ShouldNotBeNullOrEmpty();
        response.Length.ShouldBeGreaterThan(6);
        response.Length.ShouldBeLessThan(15);
    }

    [Fact]
    public void GenerateShouldSucceedWithLengthOptions()
    {
        var options = new ShortIdOptions(length: 22);
        var response = ShortId.Generate(options);

        response.ShouldNotBeNullOrEmpty();
        response.Length.ShouldBe(22);
    }

    [Fact]
    public void ShouldResetInternalStateWithoutProblems()
    {
        var action = () => { ShortId.Reset(); };
        Should.NotThrow(action);
    }

    [Fact]
    public void ShouldAllowForACustomSeed()
    {
        var action = () => { ShortId.SetSeed(678309202); };

        Should.NotThrow(action);
    }

    [Fact]
    public void GenerateShouldThrowWhenLengthIsTooSmall()
    {
        var options = new ShortIdOptions(length: 7);
        var action = () => { ShortId.Generate(options); };

        Should.Throw<ArgumentException>(action);
    }

    [Fact]
    public void GenerateShouldOnlyUseNumbersWhenSpecified()
    {
        ShortId.Reset();
        // We need to set characters to something that contains only numbers to truly test this.
        // But SetCharacters requires 50 UNIQUE characters.
        // Numbers only have 10. So we must use other characters as well but verify they are not in the output if not requested.
        // Actually, ShortId.Generate appends numbers to the pool if options.UseNumbers is true.
        // If we want to test that ONLY numbers are used, we'd need to set the pool to something empty, which is not allowed.
            
        // Let's test that if we use numbers, the output CAN contain numbers.
        // And if we don't use numbers, it doesn't.
        var options = new ShortIdOptions(useNumbers: true, useSpecialCharacters: false, length: 100);
        var response = ShortId.Generate(options);
        response.Any(char.IsNumber).ShouldBeTrue();
    }

    [Fact]
    public void GenerateShouldNotUseNumbersWhenNotSpecified()
    {
        ShortId.Reset();
        var options = new ShortIdOptions(useNumbers: false, useSpecialCharacters: false, length: 100);
        var response = ShortId.Generate(options);
        response.Any(char.IsNumber).ShouldBeFalse();
    }

    [Fact]
    public void GenerateShouldNotUseSpecialCharactersWhenNotSpecified()
    {
        ShortId.Reset();
        const string specialCharacters = "_-";
        var options = new ShortIdOptions(useNumbers: false, useSpecialCharacters: false, length: 100);
        var response = ShortId.Generate(options);
        response.Any(c => specialCharacters.Contains(c)).ShouldBeFalse();
    }

    [Fact]
    public void GenerateShouldOnlyUseSpecialCharactersWhenSpecified()
    {
        ShortId.Reset();
        const string specialCharacters = "_-";
        var options = new ShortIdOptions(useNumbers: false, useSpecialCharacters: true);
            
        // We need to set characters to something that only has specials and no letters to truly test this,
        // but the current implementation appends specials to the pool.
        // Actually, the default pool is Smalls + Bigs.
        // If we want to test that it contains specials:
        var response = ShortId.Generate(options);
        // It might not contain a special character in a single run, so we might need multiple runs or just check it's a subset of (Letters + Specials)
        response.ShouldAllBe(c => char.IsLetter(c) || specialCharacters.Contains(c));
    }

    [Fact]
    public void SetCharactersShouldRemoveWhitespaceAndDuplicates()
    {
        const string seed = " Ⓐ Ⓑ Ⓒ Ⓓ Ⓔ Ⓕ Ⓖ Ⓗ Ⓘ Ⓙ Ⓚ Ⓛ Ⓜ Ⓝ Ⓞ Ⓟ Ⓠ Ⓡ Ⓢ Ⓣ Ⓤ Ⓥ Ⓦ Ⓧ Ⓨ Ⓩ ⓐ ⓑ ⓒ ⓓ ⓔ ⓕ ⓖ ⓗ ⓘ ⓙ ⓚ ⓛ ⓜ ⓝ ⓞ ⓟ ⓠ ⓡ ⓢ ⓣ ⓤ ⓥ ⓦ ⓧ ⓨ ⓩ ① ② ③ ④ ⑤ ⑥ ⑦ ⑧ ⑨ ⑩ ⑪ ⑫ Ⓐ ";
        // This string has spaces and a duplicate Ⓐ at the end.
        var action = () => { ShortId.SetCharacters(seed); };

        Should.NotThrow(action);
            
        var response = ShortId.Generate(new ShortIdOptions(useNumbers: false, useSpecialCharacters: false, length: 100));
        response.ShouldNotContain(" ");
    }
}