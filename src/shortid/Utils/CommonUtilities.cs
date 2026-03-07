using System;
using System.Threading;

namespace shortid.Utils;

internal static class CommonUtilities
{
    private static readonly ThreadLocal<Random> Random = new(() => new Random());
    
    private static readonly DateTimeOffset ShortIdEpoch = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private const string Base85Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#";

    private const long MaxBase85 = 377_149_515_624; // 85^6 - 1

    /// <summary>
    /// Generates a random integer value within the specified range [min, max).
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number to generate.</param>
    /// <param name="max">The exclusive upper bound of the random number to generate.</param>
    /// <returns>A random integer that is greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
    public static int GenerateNumberInRange(int min, int max) => Random.Value.Next(min, max);

    /// <summary>
    /// Computes the current timestamp in deciseconds relative to a predefined epoch.
    /// </summary>
    /// <returns>The number of deciseconds elapsed since the predefined epoch.</returns>
    public static long GetTimestampInDeciseconds() => (long)(DateTimeOffset.UtcNow - ShortIdEpoch).TotalMilliseconds / 100;

    /// <summary>
    /// Encodes the given number of epochs measured in deciseconds into a Base85-encoded string.
    /// </summary>
    /// <param name="epochTimestampInDeciSeconds">The time in deciseconds since the epoch to encode.</param>
    /// <returns>A Base85-encoded string representation of the provided timestamp.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="epochTimestampInDeciSeconds"/> is negative or exceeds the maximum allowable value for Base85 encoding.
    /// </exception>
    public static string EncodeTimestamp(long epochTimestampInDeciSeconds)
    {
        // utilises C# switch expression for clean evaluation
        return epochTimestampInDeciSeconds switch
        {
            < 0 => throw new ArgumentOutOfRangeException(nameof(epochTimestampInDeciSeconds), "Input value cannot be negative."),
            <= MaxBase85 => Base85Encode(epochTimestampInDeciSeconds),
            _ => throw new ArgumentOutOfRangeException(nameof(epochTimestampInDeciSeconds), "Input exceeds maximum encodable value.")
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