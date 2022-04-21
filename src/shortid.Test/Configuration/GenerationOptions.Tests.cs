using FluentAssertions;
using shortid.Configuration;
using Xunit;

namespace shortid.Test.Configuration
{
    public class GenerationOptionsTests
    {
        [Fact]
        public void ShouldSetDefaultsOnInstantiation()
        {
            var options = new GenerationOptions();

            options
                .Length
                .Should()
                .BeGreaterThan(0);
            options
                .UseNumbers
                .Should()
                .BeFalse();
            options
                .UseSpecialCharacters
                .Should()
                .BeTrue();
        }

        [Fact]
        public void ShouldAssignRandomLengthOnInstantiation()
        {
            var options = new GenerationOptions();

            options
                .Length
                .Should()
                .BeGreaterThan(6);
            options
                .Length
                .Should()
                .BeLessThan(15);
        }

        [Fact]
        public void ShouldAllowDefaultsToBeChanged()
        {
            const bool useNumbers = true;
            const bool useSpecial = false;
            const int length = 17;

            var options = new GenerationOptions
            {
                UseNumbers = useNumbers,
                UseSpecialCharacters = useSpecial,
                Length = length
            };

            options
                .Length
                .Should()
                .Be(length);
            options
                .UseNumbers
                .Should()
                .Be(useNumbers);
            options
                .UseSpecialCharacters
                .Should()
                .Be(useSpecial);
        }
    }
}
