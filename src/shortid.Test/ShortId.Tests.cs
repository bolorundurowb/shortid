using System;
using System.Linq;
using FluentAssertions;
using shortid.Configuration;
using Xunit;

namespace shortid.Test
{
    public class ShortIdTests
    {
        [Fact]
        public void SetSeedThrowsWhenCharacterSetIsEmptyOrNull()
        {
            var seed = string.Empty;
            var action = () => { ShortId.SetCharacters(seed); };

            action
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("The replacement characters must not be null or empty.");
        }

        [Fact]
        public void SetSeedThrowsWhenCharacterSetIsLessThan20Characters()
        {
            const string seed = "783ujrcuei039kj4";
            var action = () => { ShortId.SetCharacters(seed); };

            action
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage(
                    "The replacement characters must be at least 50 letters in length and without whitespace.");
        }

        [Fact]
        public void SetSeedWorksWithValidCharSet()
        {
            const string seed = "ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ①②③④⑤⑥⑦⑧⑨⑩⑪⑫";
            var action = () => { ShortId.SetCharacters(seed); };

            action
                .Should()
                .NotThrow<InvalidOperationException>();
        }

        [Fact]
        public void GenerateThrowsWhenOptionsAreNull()
        {
            var action = () => { ShortId.Generate(null); };

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void GenerateShouldSucceedWithoutOptions()
        {
            var response = ShortId.Generate();

            response.Should().NotBeNullOrEmpty();
            response.Length.Should().BeGreaterThan(6);
            response.Length.Should().BeLessThan(15);
        }

        [Fact]
        public void GenerateShouldSucceedWithLengthOptions()
        {
            var options = new GenerationOptions(length: 22);
            var response = ShortId.Generate(options);

            response.Should().NotBeNullOrEmpty();
            response.Length.Should().Be(22);
        }

        [Fact]
        public void ShouldResetInternalStateWithoutProblems()
        {
            var action = () => { ShortId.Reset(); };
            action.Should().NotThrow();
        }

        [Fact]
        public void ShouldAllowForACustomSeed()
        {
            var action = () => { ShortId.SetSeed(678309202); };

            action.Should().NotThrow();
        }
    }
}