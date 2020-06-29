using System;

namespace shortid.Configuration
{
    public class GenerationOptions
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Determines whether numbers are used in generating the id
        /// Default: false
        /// </summary>
        public bool UseNumbers { get; set; }

        /// <summary>
        /// Determines whether special characters are used in generating the id
        /// Default: true
        /// </summary>
        public bool UseSpecialCharacters { get; set; } = true;

        /// <summary>
        /// Determines the length of the generated id
        /// Default: a random length between 7 and 15
        /// </summary>
        public int Length { get; set; } = Random.Next(7, 15);
    }
}
