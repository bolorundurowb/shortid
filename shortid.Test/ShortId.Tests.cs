using System;
using System.Linq;
using NUnit.Framework;

namespace shortid.Test
{
    [TestFixture]
    public class ShortIdTests
    {
        [Test]
        public void GenerateIsStable()
        {
            string id = string.Empty;
            Assert.DoesNotThrow(delegate
            {
                id = ShortId.Generate();
            });
            Assert.IsNotEmpty(id);
        }

        [Test]
        public void GenerateCreatesIdsWithoutNumbers()
        {
            string id = null;
            id = ShortId.Generate(false);
            Assert.IsFalse(id.Any(char.IsDigit));
        }

        [Test]
        public void GenerateCreatesIdsWithoutSpecialCharacters()
        {
            string id = null;
            id = ShortId.Generate(true, false);
            var ans = new[] {"-", "_"}.Any(x => id.Contains(x));
            Assert.IsFalse(ans); 
        }

        [Test]
        public void GenerateCreatesIdsOfASpecifiedLength()
        {
            string id = null;
            id = ShortId.Generate(false, true, 8);
            Assert.AreEqual(id.Length, 8);
        }

        [Test]
        public void SetSeedThrowsWhenCharacterSetIsEmptyOrNull()
        {
            string seed = String.Empty;
            Assert.Throws<ArgumentException>(delegate
            {
                ShortId.SetCharacters(seed);
            });
        }

        [Test]
        public void SetSeedThrowsWhenCharacterSetIsLessThan20Characters()
        {
            string seed = "783ujrcuei039kj4";
            Assert.Throws<InvalidOperationException>(delegate
            {
                ShortId.SetCharacters(seed);
            });
        }

        [Test]
        public void ResetIsStable()
        {
            Assert.DoesNotThrow(ShortId.Reset);
        }

        [Test]
        public void SetSeedIsStable()
        {
            Assert.DoesNotThrow(delegate
            {
                ShortId.Generate(0);
            });
        }
    }
}