namespace shortid.Utils;

internal static class Constants
{
    public const int MinimumOutputLength = 8;

    // to reduce the chance of collision within a million ids
    public const int MinimumAutoLength = 10;

    public const int MaximumAutoLength = 14;

    public const int MinimumCharacterSetLength = 50;

    public const string Bigs = "ABCDEFGHIJKLMNPQRSTUVWXY";

    public const string Smalls = "abcdefghjklmnopqrstuvwxyz";

    public const string Numbers = "0123456789";

    public const string Specials = "_-";
}