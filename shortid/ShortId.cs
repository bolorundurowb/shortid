using System;

namespace shortid
{
    public class ShortId
    {
        private static Random _random = new Random();
        private const string Capitals = "ABCDEFGHIJKLMNOPQRSTUVWXY";
        private const string Smalls = "abcdefghjlkmnopqrstuvwxyz";
        private const string Numbers = "0123456789";
        private const string Specials = "-_";
        private static string _pool = $"{Smalls}{Capitals}";
        
        /// <summary>
        /// Generates a random string of varying length
        /// </summary>
        /// <param name="useNumbers">Whether or not to include numbers</param>
        /// <param name="useSpecial">Whether or not special characters are included</param>
        /// <returns>A random string</returns>
        public static string Generate(bool useNumbers = false, bool useSpecial = true)
        {
            int length = _random.Next(7, 15);
            return Generate(useNumbers, useSpecial, length);
        }

        /// <summary>
        /// Generates a random string of a specified length
        /// </summary>
        /// <param name="useNumbers">Whether or not numbers are included in the string</param>
        /// <param name="useSpecial">Whether or not special characters are included</param>
        /// <param name="length">The length of the generated string</param>
        /// <returns>A random string</returns>
        public static string Generate(bool useNumbers, bool useSpecial, int length)
        {
            string pool = _pool;
            if (useNumbers)
            {
                pool = Numbers + pool;
            }
            if (useSpecial)
            {
                pool += Specials;
            }

            string output = string.Empty;
            for (int i = 0; i < length; i++)
            {
                int charIndex = _random.Next(0, pool.Length);
                output += pool[charIndex];
            }
            return output;
        }

        /// <summary>
        /// Generates a random string of a specified length
        /// </summary>
        /// <param name="length">The length of the generated string</param>
        /// <returns>A random string</returns>
        public static string Generate(int length)
        {
            return Generate(false, true, length);
        }

        public static void SetCharacters(string characters)
        {
            characters = characters.Remove(' ');
            if (characters.Length < 20)
            {
                throw new InvalidOperationException("The replacement characters must be at least 20 letters in length and without spaces.");
            }
            _pool = characters;
        }

        public static void SetSeed(int seed)
        {
            _random = new Random(seed);
        }

        public static void Reset()
        {
            _random = new Random();
            _pool = $"{Smalls}{Capitals}";
        }
    }
}