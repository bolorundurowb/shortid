using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace shortid.Test
{
    public class ShortIdTests
    {
        [Fact]
        public void GeneratedWithoutExceptions()
        {
            var id = string.Empty;
            Action action = () =>
            {
                id = ShortId.Generate();
            };
            action.ShouldNotThrow();
            id.Should().NotBeEmpty();
        }

        [Fact]
        public void GenerateCreatesIdsWithoutNumbers()
        {
            var id = ShortId.Generate(false);
            id.Any(char.IsDigit).Should().BeFalse();
        }

        [Fact]
        public void GenerateCreatesIdsWithoutSpecialCharacters()
        {
            var id = ShortId.Generate(true, false);
            var ans = new[] {"-", "_"}.Any(x => id.Contains(x));
            ans.Should().BeFalse();
        }

        [Fact]
        public void GenerateCreatesIdsOfASpecifiedLength()
        {
            var id = ShortId.Generate(false, true, 8);
            id.Length.Should().Be(8);
        }

        [Fact]
        public void SetSeedThrowsWhenCharacterSetIsEmptyOrNull()
        {
            var seed = string.Empty;
            Action action = () =>
            {
                ShortId.SetCharacters(seed);
            };
            action.ShouldThrow<ArgumentException>()
                .WithMessage("The replacement characters must not be null or empty.");
        }

        [Fact]
        public void SetSeedThrowsWhenCharacterSetIsLessThan20Characters()
        {
            const string seed = "783ujrcuei039kj4";
            Action action = () =>
            {
                ShortId.SetCharacters(seed);
            };
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("The replacement characters must be at least 20 letters in length and without whitespace.");
        }

        [Fact]
        public void ResetIsStable()
        {
            Action action = () =>
            {
                ShortId.Reset();
            };
            action.ShouldNotThrow();
        }

        [Fact]
        public void DoesNotAllowLengthsLessThan7()
        {
            Action action = () =>
            {
                ShortId.Generate(6);
            };
            action.ShouldThrowExactly<ArgumentException>()
                .WithMessage("The specified length of 6 is less than the lower limit of 7.");
        }
    }
}