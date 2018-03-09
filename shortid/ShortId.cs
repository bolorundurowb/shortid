using System;
using System.Text;

namespace shortid
{
    public static class ShortId
    {
        // app variables
        private static Random _random = new Random();
        private const string Bigs = "ABCDEFGHIJKLMNOPQRSTUVWXY";
        private const string Smalls = "abcdefghjlkmnopqrstuvwxyz";
        private const string Numbers = "0123456789";
        private const string Specials = "-_";
        private static string _pool = $"{Smalls}{Bigs}";
        
        // thread management variables
        private static readonly object threadLock = new object();

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
        /// Generates a random string of a specified length with the option to add numbers and special characters
        /// </summary>
        /// <param name="useNumbers">Whether or not numbers are included in the string</param>
        /// <param name="useSpecial">Whether or not special characters are included</param>
        /// <param name="length">The length of the generated string</param>
        /// <returns>A random string</returns>
        public static string Generate(bool useNumbers, bool useSpecial, int length)
        {
            if (length < 7)
            {
                throw new ArgumentException($"The specified length of {length} is less than the lower limit of 7.");
            }
            
            string __pool;
            Random rand;
            
            lock (threadLock)
            {
                __pool = _pool;
                rand = _random;
            }
            
            StringBuilder poolBuilder = new StringBuilder(__pool);
            if (useNumbers)
            {
                poolBuilder.Append(Numbers);
            }
            if (useSpecial)
            {
                poolBuilder.Append(Specials);
            }

            string pool = poolBuilder.ToString();
            
            char[] output = new char[length];
            for (int i = 0; i < length; i++)
            {
                int charIndex = rand.Next(0, pool.Length);
                output[i] =  pool[charIndex];
            }
            return new string(output);
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

        /// <summary>
        /// Changes the character set that id's are generated from
        /// </summary>
        /// <param name="characters">The new character set</param>
        /// <exception cref="InvalidOperationException">Thrown when the new character set is less than 20 characters</exception>
        public static void SetCharacters(string characters)
        {
            if (string.IsNullOrWhiteSpace(characters))
            {
                throw new ArgumentException("The replacement characters must not be null or empty.");
            }
            
            var stringBuilder = new StringBuilder();
            foreach (var character in characters)
            {
                if (!char.IsWhiteSpace(character)) {
                    stringBuilder.Append(character);
                }
            }
            
            if (stringBuilder.Length < 20)
            {
                throw new InvalidOperationException(
                    "The replacement characters must be at least 20 letters in length and without whitespace.");
            }
            
            _pool = stringBuilder.ToString();
        }

        /// <summary>
        /// Sets the seed that the random generator works with.
        /// </summary>
        /// <param name="seed">The seed for the random number generator</param>
        public static void SetSeed(int seed)
        {
            lock (threadLock)
            {
                _random = new Random(seed);
            }
        }

        /// <summary>
        /// Resets the random number generator and character set
        /// </summary>
        public static void Reset()
        {
            lock (threadLock)
            {
                _random = new Random();
                _pool = $"{Smalls}{Bigs}";
            }
        }
    }
}