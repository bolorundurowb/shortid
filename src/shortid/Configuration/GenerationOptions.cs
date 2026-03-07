using shortid.Utils;

namespace shortid.Configuration;

/// <summary>
/// Represents options for configuring the generation of unique identifiers in the generation process.
/// </summary>
public class GenerationOptions(
    bool useNumbers = false,
    bool useSpecialCharacters = true,
    int? length = null,
    bool generateMonotonic = false)
{
    /// <summary>
    /// Determines whether numbers are used in generating the id.
    /// Default: false.
    /// </summary>
    public bool UseNumbers { get; } = useNumbers;

    /// <summary>
    /// Determines whether special characters are used in generating the id.
    /// Default: true.
    /// </summary>
    public bool UseSpecialCharacters { get; } = useSpecialCharacters;

    /// <summary>
    /// Determines the length of the generated id.
    /// Default: a random generated id length between 8 and 14 characters.
    /// </summary>
    public int Length { get; } = length ?? RandomUtils.GenerateNumberInRange(Constants.MinimumAutoLength, Constants.MaximumAutoLength);

    /// <summary>
    /// Specifies whether the generated id should follow a monotonic sequence, ensuring
    /// that each id is greater than the previous one. This can help maintain a predictable
    /// and sequential ordering of ids.
    /// Default: false.
    /// </summary>
    public bool GenerateMonotonic { get; } = generateMonotonic;
}