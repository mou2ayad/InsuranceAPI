using FluentAssertions;
using Insurance.Api.Services;
using Insurance.Tests.Builders;
using Insurance.Tests.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Insurance.Tests.Test
{
    public class InsurancePriceStrategyTest
    {
        [Theory]
        [InlineData(499, 0)]
        [InlineData(500, 1000)]
        [InlineData(800, 1000)]
        [InlineData(2000, 2000)]
        [InlineData(3000, 2000)]
        public void GivenProductPrice_ShouldReturnExpectedInsuranceValue_BasedOnInsurancePriceRules(double productPrice,
            double expectedInsuranceValue)
        {
            var sut = Sut();
            var product = ProductBuilder.Create().WithPrice(productPrice).Build();

            var insurance = sut.GetInsuranceValue(product);

            insurance.Should().Be(expectedInsuranceValue);
        }

        [Fact]
        public void InsurancePriceStrategy_ShouldLoadPriceRulesFromConfig()
        {
            var config = BuildConfigurationFromConfigFile();
            var sut = TestableInsurancePriceStrategy.Create(config);

            var rules= sut.GetRules();

            rules.Count.Should().Be(3);
            rules[0].LessThan.Should().Be(500);
            rules[1].LessThan.Should().Be(2000);
            rules[2].MoreThanOrEqual.Should().Be(2000);

        }



        public InsurancePriceStrategy Sut() => TestableInsurancePriceStrategy.Create()
            .WithRules(InsuranceStrategiesHelper.BuildInsurancePriceRules());

        private IConfiguration BuildConfigurationFromConfigFile() =>
            new ConfigurationBuilder().AddJsonFile("InsurancePriceRules.json").Build();
    }
}
