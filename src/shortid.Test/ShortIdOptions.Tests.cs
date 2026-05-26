using shortid.Utils;
using OmniAssert;
using Xunit;

namespace shortid.Test;

public class ShortIdOptionsTests
{
    [Fact]
    public void Constructor_WithoutParameters_SetsExpectedBooleanDefaults()
    {
        var options = new ShortIdOptions();

        options
            .Length
            .Verify().ToBeGreaterThan(0);
        options
            .UseNumbers
            .Verify().ToBeFalse();
        options
            .UseSpecialCharacters
            .Verify().ToBeTrue();
    }

    [Fact]
    public void Constructor_WithoutParameters_SetsDefaultOutputLength()
    {
        var options = new ShortIdOptions();

        options.Length.Verify().ToBe(Constants.DefaultOutputLength);
    }

    [Fact]
    public void Constructor_WithParameters_AppliesSuppliedValues()
    {
        const bool useNumbers = true;
        const bool useSpecial = false;
        const int length = 17;

        var options = new ShortIdOptions(useNumbers, useSpecial, length);

        options
            .Length
            .Verify().ToBe(length);
        options
            .UseNumbers
            .Verify().ToBeTrue();
        options
            .UseSpecialCharacters
            .Verify().ToBeFalse();
    }
}