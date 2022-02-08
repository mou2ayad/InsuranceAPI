using System.Threading.Tasks;
using FluentAssertions;
using Insurance.Api.Contracts;
using Insurance.Api.Services;
using Insurance.DataProvider.Contract;
using Insurance.Tests.Utils;
using Xunit;

namespace Insurance.Tests.Services
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
        [InlineData(499, true, 500, "Smartphones")]
        [InlineData(500, true, 1500, "Smartphones")]
        [InlineData(800, true, 1500, "Smartphones")]
        [InlineData(2000, true, 2500, "Laptops")]
        [InlineData(3000, true, 2500, "Laptops")]
        [InlineData(3000, false, 0, "Laptops")]
        public async Task GivenProductRequiresAdditional_ShouldReturnExtra500ToInsuranceCost(double price, bool canBeInsured, double expected,string productTypeName)
        {
            var productDataClient = CreateProductDataClient()
                .WithProductSalePrice(price)
                .WithProductTypeCanBeInsured(canBeInsured)
                .WithProductTypeName(productTypeName);
            var sut = Sut(productDataClient);

            var response = await sut.CalculateInsurance(100);
            response.InsuranceValue.Should().Be(expected);
        }

        private FakeProductDataApiClient CreateProductDataClient() => 
            FakeProductDataApiClient.Create();

        private IInsuranceCostService Sut(IProductDataApiClient productDataApiClient ) 
            => new InsuranceCostService(productDataApiClient, FakeLogger<InsuranceCostService>.Create());
    }
}
