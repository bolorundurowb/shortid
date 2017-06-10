using System;

namespace shortid
{
    public class ShortId
    {
        private static Random _random = new Random();
        private const string capitals = "ABCDEFGHIJKLMNOPQRSTUVWXY";
        private const string smalls = "abcdefghjlkmnopqrstuvwxyz";
        private const string numbers = "0123456789";
        private const string specials = "-_";
        
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
            string pool = $"{capitals}{smalls}";
            if (useNumbers)
            {
                pool += numbers;
            }
            if (useSpecial)
            {
                pool += specials;
            }
            
            string output = string.Empty;
            for (int i = 0; i < length; i++)
            {
                int charIndex = _random.Next(0, pool.Length);
                output += pool[charIndex];
            }
            return output;
         }
    }
}