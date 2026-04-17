namespace shortid.Utils;

internal static class Constants
{
    public const int MinimumOutputLength = 8;

    /// <summary>Default ID length when no explicit length is passed to <c>ShortIdOptions</c>.</summary>
    public const int DefaultOutputLength = 15;

    public const int MinimumCharacterSetLength = 50;

    public const string Bigs = "ABCDEFGHIJKLMNPQRSTUVWXY";

    public const string Smalls = "abcdefghjklmnopqrstuvwxyz";

    public const string Numbers = "0123456789";

    public const string Specials = "_-";
}