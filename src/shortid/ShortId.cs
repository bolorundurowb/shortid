using System;
using System.Linq;
using shortid.Utils;

namespace shortid;

public static class ShortId
{
    // app variables
    private static Random _random = new();
    private const string Bigs = "ABCDEFGHIJKLMNPQRSTUVWXY";
    private const string Smalls = "abcdefghjklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const string Specials = "_-";
    private static string _pool = $"{Smalls}{Bigs}";

    // thread management variables
    private static readonly object ThreadLock = new();

    /// <summary>
    /// Generates a random string to match the default generation options.
    /// i.e random length between 8 and 14, with special characters and no numbers
    /// </summary>
    /// <returns>A random unique string.</returns>
    public static string Generate() => Generate(new ShortIdOptions());

    /// <summary>
    /// Generates a random string to match the specified options.
    /// </summary>
    /// <param name="options">The generation options.</param>
    /// <returns>A random unique string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when options is null.</exception>
    /// <exception cref="ArgumentException">Thrown when options.Length is less than 8.</exception>
    public static string Generate(ShortIdOptions options)
    {
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        if (options.Length < Constants.MinimumAutoLength)
            throw new ArgumentException($"Length must be at least {Constants.MinimumAutoLength}.");

        var currentPool = _pool;
        Span<char> buffer = stackalloc char[options.Length];
        var currentIndex = 0;
        
        if (options.GenerateSequential)
        {
            // Granularity changed to milliseconds
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var prefix = CommonUtilities.EncodeTimestamp(timestamp);

            // Efficiently copy the prefix using Span
            prefix.AsSpan().CopyTo(buffer);
            currentIndex = prefix.Length;
        }
        
        // Avoid StringBuilder allocation for pool construction
        var activePool = currentPool;
        if (options.UseNumbers || options.UseSpecialCharacters)
        {
            activePool += options.UseNumbers ? Numbers : string.Empty;
            activePool += options.UseSpecialCharacters ? Specials : string.Empty;
        }

        var poolSpan = activePool.AsSpan();

        // Generate random characters directly into the stack buffer
        for (var i = currentIndex; i < buffer.Length; i++)
        {
            buffer[i] = poolSpan[_random.Next(poolSpan.Length)];
        }

        // Create the final string directly from the Span
        return new string(buffer.ToArray());
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
            throw new ArgumentException("The replacement characters must not be null or empty.");

        var charSet = characters
            .ToCharArray()
            .Where(x => !char.IsWhiteSpace(x))
            .Distinct()
            .ToArray();

        if (charSet.Length < Constants.MinimumCharacterSetLength)
            throw new InvalidOperationException(
                $"The replacement characters must be at least {Constants.MinimumCharacterSetLength} letters in length and without whitespace.");

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