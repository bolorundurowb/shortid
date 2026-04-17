using shortid.Utils;
using Shouldly;
using Xunit;

namespace shortid.Test;

public class ShortIdOptionsTests
{
    [Fact]
    public void ShouldSetDefaultsOnInstantiation()
    {
        var options = new ShortIdOptions();

        options
            .Length
            .ShouldBeGreaterThan(0);
        options
            .UseNumbers
            .ShouldBeFalse();
        options
            .UseSpecialCharacters
            .ShouldBeTrue();
    }

    [Fact]
    public void ShouldAssignDefaultLengthOnInstantiation()
    {
        var options = new ShortIdOptions();

        options.Length.ShouldBe(Constants.DefaultOutputLength);
    }

    [Fact]
    public void ShouldAllowDefaultsToBeChanged()
    {
        const bool useNumbers = true;
        const bool useSpecial = false;
        const int length = 17;

        var options = new ShortIdOptions(useNumbers, useSpecial, length);

        options
            .Length
            .ShouldBe(length);
        options
            .UseNumbers
            .ShouldBe(useNumbers);
        options
            .UseSpecialCharacters
            .ShouldBe(useSpecial);
    }
}