using System;
using System.Linq;
using System.Text;
using shortid.Configuration;
using shortid.Utils;

namespace shortid
{
    public static class ShortId
    {
        // app variables
        private static Random _random = new Random();
        private const string Bigs = "ABCDEFGHIJKLMNPQRSTUVWXY";
        private const string Smalls = "abcdefghjklmnopqrstuvwxyz";
        private const string Numbers = "0123456789";
        private const string Specials = "_-";
        private static string _pool = $"{Smalls}{Bigs}";

        // thread management variables
        private static readonly object ThreadLock = new object();

        /// <summary>
        /// Generates a random string of varying length with special characters and without numbers.
        /// </summary>
        /// <param name="useNumbers">Whether or not to include numbers.</param>
        /// <param name="useSpecial">Whether or not special characters are included.</param>
        /// <returns>A random string.</returns>
        [Obsolete("Use the Generate(options) overload instead")]
        public static string Generate(bool useNumbers = false, bool useSpecial = true)
        {
            var options = new GenerationOptions
            {
                UseNumbers = useNumbers,
                UseSpecialCharacters = useSpecial
            };

            return Generate(options);
        }

        /// <summary>
        /// Generates a random string of a specified length with the option to add numbers and special characters.
        /// </summary>
        /// <param name="useNumbers">Whether or not numbers are included in the string.</param>
        /// <param name="useSpecial">Whether or not special characters are included.</param>
        /// <param name="length">The length of the generated string.</param>
        /// <returns>A random string.</returns>
        [Obsolete("Use the Generate(options) overload instead")]
        public static string Generate(bool useNumbers, bool useSpecial, int length)
        {
            var options = new GenerationOptions
            {
                UseNumbers = useNumbers,
                UseSpecialCharacters = useSpecial,
                Length = length
            };

            return Generate(options);
        }

        /// <summary>
        /// Generates a random string of a specified length with special characters and without numbers.
        /// </summary>
        /// <param name="length">The length of the generated string.</param>
        /// <returns>A random string.</returns>
        [Obsolete("Use the Generate(options) overload instead")]
        public static string Generate(int length)
        {
            var options = new GenerationOptions
            {
                Length = length
            };

            return Generate(options);
        }

        /// <summary>
        /// Generates a random string to match the specified options.
        /// </summary>
        /// <param name="options">The generation options.</param>
        /// <returns>A random string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when options is null.</exception>
        /// <exception cref="ArgumentException">Thrown when options.Length is less than 8.</exception>
        public static string Generate(GenerationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.Length < Constants.MinimumAutoLength)
            {
                throw new ArgumentException(
                    $"The specified length of {options.Length} is less than the lower limit of {Constants.MinimumAutoLength} to avoid conflicts.");
            }

            var characterPool = _pool;
            var poolBuilder = new StringBuilder(characterPool);
            if (options.UseNumbers)
            {
                poolBuilder.Append(Numbers);
            }

            if (options.UseSpecialCharacters)
            {
                poolBuilder.Append(Specials);
            }

            var pool = poolBuilder.ToString();

            var output = new char[options.Length];
            for (var i = 0; i < options.Length; i++)
            {
                lock (ThreadLock)
                {
                    var charIndex = _random.Next(0, pool.Length);
                    output[i] = pool[charIndex];
                }
            }

            return new string(output);
        }

        /// <summary>
        /// Changes the character set that id's are generated from.
        /// </summary>
        /// <param name="characters">The new character set.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="characters"/> is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the new character set is less than 50 characters.</exception>
        public static void SetCharacters(string characters)
        {
            if (string.IsNullOrWhiteSpace(characters))
            {
                throw new ArgumentException("The replacement characters must not be null or empty.");
            }

            var charSet = characters
                .ToCharArray()
                .Where(x => !char.IsWhiteSpace(x))
                .Distinct()
                .ToArray();

            if (charSet.Length < Constants.MinimumCharacterSetLength)
            {
                throw new InvalidOperationException(
                    $"The replacement characters must be at least {Constants.MinimumCharacterSetLength} letters in length and without whitespace.");
            }

            lock (ThreadLock)
            {
                _pool = new string(charSet);
            }
        }

        /// <summary>
        /// Sets the seed that the random generator works with.
        /// </summary>
        /// <param name="seed">The seed for the random number generator.</param>
        public static void SetSeed(int seed)
        {
            lock (ThreadLock)
            {
                _random = new Random(seed);
            }
        }

        /// <summary>
        /// Resets the random number generator and character set.
        /// </summary>
        public static void Reset()
        {
            lock (ThreadLock)
            {
                _random = new Random();
                _pool = $"{Smalls}{Bigs}";
            }
        }
    }
}