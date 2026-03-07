using System;
using System.Threading;

namespace shortid.Utils;

internal static class CommonUtilities
{
    private static readonly ThreadLocal<Random> Random = new(() => new Random());

    /*
     * We chose January 1, 2026 as the ShortId epoch instead of the standard Unix because it buys us
     * a bit more time to make the library more robust against timestamp collisions.
     */
    private static readonly DateTimeOffset ShortIdEpoch = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private const string Base85Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#";

    /*
     * MaxBase85 is 85^6 - 1, which equals 377,149,515,624.
     * We chose 6 characters to represent the time component because it's the maximum length we can
     * use if the user chooses the minimum ID length of 8 characters, while still leaving 2 random characters
     * for entropy.
     */
    private const long MaxBase85 = 377_149_515_624;

    /// <summary>
    /// Generates a random integer value within the specified range [min, max).
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number to generate.</param>
    /// <param name="max">The exclusive upper bound of the random number to generate.</param>
    /// <returns>A random integer that is greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
    public static int GenerateNumberInRange(int min, int max) => Random.Value.Next(min, max);

    /// <summary>
    /// Retrieves the current timestamp in centiseconds relative to the predefined ShortId epoch.
    /// </summary>
    /// <returns>A long integer representing the number of centiseconds elapsed since the ShortId epoch.</returns>
    public static long GetTimestampInCentiseconds()
    {
        /*
         * We chose centiseconds as our resolution because it allows the library to cater to over 119 years
         * of generated values within the MaxBase85 constraint (377,149,515,624 / 100 / 60 / 60 / 24 / 365.25 ≈ 119.5).
         */
        return (long)(DateTimeOffset.UtcNow - ShortIdEpoch).TotalMilliseconds / 10;
    }

    /// <summary>
    /// Encodes a timestamp in centiseconds into a Base85 encoded string representation.
    /// </summary>
    /// <param name="epochTimestampInCentiseconds">The timestamp in centiseconds to encode. Must be non-negative and less than or equal to the maximum encodable value.</param>
    /// <returns>A Base85 encoded string representation of the input timestamp.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the input timestamp is negative or exceeds the maximum encodable value.</exception>
    public static string EncodeTimestamp(long epochTimestampInCentiseconds)
    {
        // utilises C# switch expression for clean evaluation
        return epochTimestampInCentiseconds switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(epochTimestampInCentiseconds), "Input value cannot be negative."),
            <= MaxBase85 => Base85Encode(epochTimestampInCentiseconds),
            _ => throw new ArgumentOutOfRangeException(nameof(epochTimestampInCentiseconds), "Input exceeds maximum encodable value.")
        };
    }

    private static string Base85Encode(long value)
    {
        const int targetBase = 85;
        var buffer = new char[6];
        var index = 5;

        do
        {
            buffer[index--] = Base85Alphabet[(int)(value % targetBase)];
            value /= targetBase;
        } while (value > 0);

        // Pad the remainder with the first alphabet character
        while (index >= 0)
        {
            buffer[index--] = Base85Alphabet[0];
        }

        return new string(buffer);
    }
}