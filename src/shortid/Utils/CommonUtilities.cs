using System;
using System.Threading;

namespace shortid.Utils;

internal static class CommonUtilities
{
    private static readonly ThreadLocal<Random> Random = new(() => new Random());
    
    private const string Base62Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    // Using the Z85 standard alphabet for the remaining 23 characters
    private const string Base85Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#";

    private const long MaxBase62 = 916_132_831;   // 62^5 - 1
    private const long MaxBase85 = 4_437_053_124; // 85^5 - 1

    /// <summary>
    /// Generates a random integer value within the specified range [min, max).
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number to generate.</param>
    /// <param name="max">The exclusive upper bound of the random number to generate.</param>
    /// <returns>A random integer that is greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
    public static int GenerateNumberInRange(int min, int max) => Random.Value.Next(min, max);

    /// <summary>
    /// Encodes a Unix timestamp in seconds into a string representation using a specified base
    /// encoding (Base62 or Base85) depending on the timestamp's value.
    /// </summary>
    /// <param name="unixTimestampInSeconds">The Unix timestamp in seconds to encode. Must be non-negative.</param>
    /// <returns>A string representation of the encoded timestamp. The encoding will use Base62 for
    /// values up to 62^5 - 1 and Base85 for values up to 85^5 - 1.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="unixTimestampInSeconds"/> is negative or exceeds the maximum allowable
    /// value for Base85 encoding.
    /// </exception>
    public static string EncodeTimestamp(long unixTimestampInSeconds)
    {
        // Utilises C# switch expression for clean evaluation
        return unixTimestampInSeconds switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(unixTimestampInSeconds), "Value cannot be negative."),
            <= MaxBase62 => EncodeWithAlphabet(unixTimestampInSeconds, Base62Alphabet, 62),
            <= MaxBase85 => EncodeWithAlphabet(unixTimestampInSeconds, Base85Alphabet, 85),
            _ => throw new ArgumentOutOfRangeException(nameof(unixTimestampInSeconds), $"Value exceeds .")
        };
    }

    private static string EncodeWithAlphabet(long value, string alphabet, int targetBase)
    {
        char[] buffer = new char[5];
        int index = 4;

        do
        {
            buffer[index--] = alphabet[(int)(value % targetBase)];
            value /= targetBase;
        } while (value > 0);

        // Pad the remainder with the first alphabet character
        while (index >= 0)
        {
            buffer[index--] = alphabet[0];
        }

        // Return exactly 4 characters if the first padded character matches the zero-value, otherwise 5
        return buffer[0] == alphabet[0] ? new string(buffer, 1, 4) : new string(buffer);
    }
}