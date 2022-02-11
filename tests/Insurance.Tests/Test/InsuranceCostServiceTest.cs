using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Insurance.Api.Contracts;
using Insurance.Api.Services;
using Insurance.DataProvider.Contract;
using Insurance.Tests.Builders;
using Insurance.Tests.Services;
using Insurance.Tests.Utils;
using Xunit;

namespace Insurance.Tests.Test
{
    public class InsuranceCostServiceTest 
    {
        [Theory]
        [InlineData(499, true, 0)]
        [InlineData(500, true, 1000)]
        [InlineData(800, true, 1000)]
        [InlineData(2000, true, 2000)]
        [InlineData(3000, true, 2000)]
        [InlineData(3000, false, 0)]
        public async Task GivenValidProductId_ShouldReturnCorrectInsuranceCost(double price,bool canBeInsured,double expected)
        {
            var productDataClient = CreateProductDataClient()
                .WithProductSalePrice(price)
                .WithProductTypeCanBeInsured(canBeInsured);
            var sut = Sut(productDataClient);

            var response=await sut.CalculateInsurance(100);
            response.InsuranceValue.Should().Be(expected);
        }

        [Theory]
        [InlineData(499, true, 500, ProductTypeSamples.Laptops)]
        [InlineData(500, true, 1500, ProductTypeSamples.Laptops)]
        [InlineData(800, true, 1500, ProductTypeSamples.Laptops)]
        [InlineData(2000, true, 2500, ProductTypeSamples.Smartphones)]
        [InlineData(3000, true, 2500, ProductTypeSamples.Smartphones)]
        [InlineData(3000, false, 0, ProductTypeSamples.Smartphones)]
        public async Task GivenProductRequiresSurchargeRate_ShouldReturnExtra500ToInsuranceCost(double price,
            bool canBeInsured, double expected, int productTypeId)
        {
            int productId = 10;
            var productDataClient = CreateProductDataClient()
                .WithProductSalePrice(price)
                .WithProductTypeCanBeInsured(canBeInsured)
                .WithProductTypeId(productTypeId)
                .WithProductId(productId);
            var sut = Sut(productDataClient);

            var response = await sut.CalculateInsurance(productId);

            response.InsuranceValue.Should().Be(expected);
        }

        [Theory]
        [InlineData(499, true, 0)]
        [InlineData(500, true, 1000)]
        [InlineData(800, true, 1000)]
        [InlineData(2000, true, 2000)]
        [InlineData(3000, true, 2000)]
        [InlineData(3000, false, 0)]
        public async Task GivenProductNotRequireSurchargeRate_ShouldReturnInsuranceCostWithoutExtraRate(double price,
            bool canBeInsured, double expected)
        {
            int productId = 10;
            var productDataClient = CreateProductDataClient()
                .WithProductSalePrice(price)
                .WithProductTypeCanBeInsured(canBeInsured)
                .WithProductTypeId(ProductTypeSamples.MP3Players)
                .WithProductId(productId);
            var sut = Sut(productDataClient);

            var response = await sut.CalculateInsurance(productId);

            response.InsuranceValue.Should().Be(expected);
        }

        private FakeProductDataApiClient CreateProductDataClient() => 
            FakeProductDataApiClient.Create();

        private IProductInsuranceCostService Sut(IProductDataApiClient productDataApiClient ) 
            => new ProductInsuranceCostService(productDataApiClient,GetStrategies());

        private IEnumerable<IInsuranceStrategy> GetStrategies() =>
            new List<IInsuranceStrategy>
            {
                TestableInsurancePriceStrategy.Create()
                    .WithRules(InsuranceStrategiesHelper.BuildInsurancePriceRules()),
                new InsuranceProductTypeStrategy(MockedInsuranceSurchargeRatesRepository.Create(500))
            };

       
    }
}
