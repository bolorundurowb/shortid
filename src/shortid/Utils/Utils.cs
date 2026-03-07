using System;
using System.Diagnostics;
using System.Threading;

namespace shortid.Utils;

internal static class Utils
{
    private static readonly DateTimeOffset UnixEpoch = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private static readonly long EpochTicks = UnixEpoch.Ticks;
    
    // Synchronise every 1 hour to prevent drift
    private static readonly long SyncIntervalTimerTicks = Stopwatch.Frequency * 3600;

    private static long _baseTimerTicks = Stopwatch.GetTimestamp();
    private static long _baseUtcTicks = DateTimeOffset.UtcNow.Ticks;
    private static readonly object _syncLock = new();
    
    private static readonly ThreadLocal<Random> Random = new(() => new Random());

    /// <summary>
    /// Generates a random integer value within the specified range [min, max).
    /// </summary>
    /// <param name="min">The inclusive lower bound of the random number to generate.</param>
    /// <param name="max">The exclusive upper bound of the random number to generate.</param>
    /// <returns>A random integer that is greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
    public static int GenerateNumberInRange(int min, int max) => Random.Value.Next(min, max);

    /// <summary>
    /// Retrieves the current Unix time in nanoseconds since the Unix epoch (January 1, 1970, 00:00:00 UTC).
    /// </summary>
    /// <returns>The current Unix time in nanoseconds as a 64-bit integer.</returns>
    public static long GetUnixTimeNanoseconds()
    {
        if (Stopwatch.IsHighResolution)
        {
            var currentTimerTicks = Stopwatch.GetTimestamp();
            var baseTimer = Volatile.Read(ref _baseTimerTicks);
            var elapsedTimerTicks = currentTimerTicks - baseTimer;

            if (elapsedTimerTicks >= SyncIntervalTimerTicks)
            {
                lock (_syncLock)
                {
                    currentTimerTicks = Stopwatch.GetTimestamp();
                    elapsedTimerTicks = currentTimerTicks - _baseTimerTicks;

                    if (elapsedTimerTicks >= SyncIntervalTimerTicks)
                    {
                        _baseTimerTicks = currentTimerTicks;
                        _baseUtcTicks = DateTimeOffset.UtcNow.Ticks;
                        elapsedTimerTicks = 0;
                    }
                }
            }

            var elapsedNanoseconds =
                (elapsedTimerTicks * 1_000_000_000L) / Stopwatch.Frequency;

            var baseUnixNanoseconds =
                (Volatile.Read(ref _baseUtcTicks) - EpochTicks) * 100;

            return baseUnixNanoseconds + elapsedNanoseconds;
        }

        // Fallback to DateTimeOffset Ticks (1 tick = 100 nanoseconds)
        return (DateTimeOffset.UtcNow.Ticks - EpochTicks) * 100;
    }
}