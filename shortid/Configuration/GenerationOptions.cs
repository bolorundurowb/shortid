using System;

namespace shortid.Configuration
{
    public class GenerationOptions
    {
        private static readonly Random Random = new Random();

        public bool UseNumbers { get; set; }

        public bool UseSpecialCharacters { get; set; }

        public int Length { get; set; } = Random.Next(7, 15);
    }
}
