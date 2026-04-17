using shortid.Utils;

namespace shortid;

/// <summary>
/// Represents options for configuring the generation of unique identifiers in the generation process.
/// </summary>
public class ShortIdOptions(
    bool useNumbers = false,
    bool useSpecialCharacters = true,
    int? length = null,
    bool generateSequential = false)
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
    /// Default: 15 characters (<see cref="Constants.DefaultOutputLength"/>) when no length is specified.
    /// </summary>
    public int Length { get; } = length ?? Constants.DefaultOutputLength;

    /// <summary>
    /// Specifies whether the generated id should follow a monotonic sequence, ensuring
    /// that each id is greater than the previous one. This can help maintain a predictable
    /// and sequential ordering of ids.
    /// Default: false.
    /// </summary>
    public bool GenerateSequential { get; } = generateSequential;
}