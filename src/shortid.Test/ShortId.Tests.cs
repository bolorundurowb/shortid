using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace shortid.Test
{
    public class ShortIdTests
    {

        [Fact]
        public void SetSeedThrowsWhenCharacterSetIsEmptyOrNull()
        {
            var seed = string.Empty;
            Action action = () => { ShortId.SetCharacters(seed); };

            action
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("The replacement characters must not be null or empty.");
        }

        [Fact]
        public void SetSeedThrowsWhenCharacterSetIsLessThan20Characters()
        {
            const string seed = "783ujrcuei039kj4";
            Action action = () => { ShortId.SetCharacters(seed); };

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
            Action action = () => { ShortId.SetCharacters(seed); };

            action
                .Should()
                .NotThrow<InvalidOperationException>();
        }

        [Fact]
        public void SetSeedThrowsWhenOptionsAreNull()
        {
            Action action = () => { ShortId.Generate(null); };

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldResetInternalStateWithoutProblems()
        {
            Action action = () => { ShortId.Reset(); };
            action.Should().NotThrow();
        }

        [Fact]
        public void ShouldAllowForACustomSeed()
        {
            Action action = () => { ShortId.SetSeed(678309202); };

            action.Should()
                .NotThrow<Exception>();
        }
    }
}
