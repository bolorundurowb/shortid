using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace shortid.Test
{
    public class ShortIdTests
    {
        [Fact]
        public void GenerateIsStable()
        {
            string id = string.Empty;
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
            string id = null;
            id = ShortId.Generate(false);
            id.Any(char.IsDigit).Should().BeFalse();
        }

        [Fact]
        public void GenerateCreatesIdsWithoutSpecialCharacters()
        {
            string id = null;
            id = ShortId.Generate(true, false);
            var ans = new[] {"-", "_"}.Any(x => id.Contains(x));
            ans.Should().BeFalse();
        }

        [Fact]
        public void GenerateCreatesIdsOfASpecifiedLength()
        {
            string id = null;
            id = ShortId.Generate(false, true, 8);
            id.Length.Should().Be(8);
        }

        [Fact]
        public void SetSeedThrowsWhenCharacterSetIsEmptyOrNull()
        {
            string seed = String.Empty;
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
            string seed = "783ujrcuei039kj4";
            Action action = () =>
            {
                ShortId.SetCharacters(seed);
            };
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("The replacement characters must be at least 20 letters in length and without spaces.");
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
        public void SetSeedIsStable()
        {
            Action action = () =>
            {
                ShortId.Generate(0);
            };
            action.ShouldNotThrow();
        }
    }
}