using System;

namespace shortid
{
    public class ShortId
    {
        private static Random _random = new Random();
        private const string capitals = "ABCDEFGHIJKLMNOPQRSTUVWXY";
        private const string smalls = "abcdefghjlkmnopqrstuvwxyz";
        private const string numbers = "0123456789";
        
        /// <summary>
        /// Generates a random string of varying length
        /// </summary>
        /// <param name="useNumbers">Whether or not to include numbers</param>
        /// <returns>A random string</returns>
        public static string Generate(bool useNumbers = false)
        {
            int length = _random.Next(7, 15);
            return Generate(useNumbers, length);
        }

        /// <summary>
        /// Generates a random string of a specified length
        /// </summary>
        /// <param name="useNumbers">Whether or not numbers are included in the string</param>
        /// <param name="length">The length of the generated string</param>
        /// <returns>A random string</returns>
        public static string Generate(bool useNumbers, int length)
        {
            string pool = $"{capitals}{smalls}";
            if (useNumbers)
            {
                pool += numbers;
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