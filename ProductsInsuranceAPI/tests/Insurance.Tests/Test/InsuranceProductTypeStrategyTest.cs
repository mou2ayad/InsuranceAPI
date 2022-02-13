using FluentAssertions;
using Insurance.Api.Contracts;
using Insurance.Api.Services;
using Insurance.Tests.Builders;
using Insurance.Tests.Services;
using Xunit;

namespace Insurance.Tests.Test
{
    public class InsuranceProductTypeStrategyTest
    {
        [Fact]
        public void GivenProductWithProductTypeHavingSurchargeRate_ShouldReturnCorrectSurchargeRate()
        {
            int laptopsProductType = 32;
            var product = ProductBuilder.Create().WithProductTypeId(laptopsProductType).Build();
            var expectedInsurance = 500;
            var insuranceSurchargeRatesRepository = MockedInsuranceSurchargeRatesRepository.Create(expectedInsurance);
            var sut = Sut(insuranceSurchargeRatesRepository);

            var insurance=sut.GetInsuranceValue(product);

            insurance.Should().Be(expectedInsurance);

        }

        [Fact]
        public void GivenProductWithProductTypeDoesNotHaveSurchargeRate_ShouldNotReturnAnySurchargeRate()
        {
            int MP3PlayersProductTypeId=12;
            var product = ProductBuilder.Create().WithProductTypeId(MP3PlayersProductTypeId).Build();
            var surchargeRate = 500;
            var insuranceSurchargeRatesRepository = MockedInsuranceSurchargeRatesRepository.Create(surchargeRate);
            var sut = Sut(insuranceSurchargeRatesRepository);

            var insurance = sut.GetInsuranceValue(product);

            insurance.Should().NotBe(surchargeRate);

        }

        private InsuranceProductTypeStrategy Sut(IInsuranceSurchargeRatesRepository insuranceSurchargeRatesRepository) =>
            new (insuranceSurchargeRatesRepository);

    }
}
