using System;

namespace shortid
{
    public class ShortId
    {
        private static Random _random = new Random();
        private const string capitals = "ABCDEFGHIJKLMNOPQRSTUVWXY";
        private const string smalls = "abcdefghjlkmnopqrstuvwxyz";
        private const string numbers = "0123456789";
        
        public static string Generate(bool useNumbers = false)
        {
            int length = _random.Next(7, 15);
            return Generate(useNumbers, length);
        }

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