using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Insurance.Api.Contracts;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Insurance.DataProvider.Contract;
using Insurance.DataProvider.Model;
using Insurance.Tests.Builders;
using Insurance.Tests.Services;
using Insurance.Tests.Utils;
using Xunit;

namespace Insurance.Tests.Test
{
    public class OrderInsuranceCalculatorServiceTest
    {
        [Fact]
        public async Task GivenOneProductTypeInTheOrderWithQuantity_ReturnsCorrectInsuranceValue()
        {
            int productId = 10;
            int insurance = 500;
            int quantity = 2;
            var productInsuranceCostService = FakeProductInsuranceCostService.Create().With(productId, insurance);
            var insuranceRequest = OrderInsuranceRequestBuilder.Create().With(productId, quantity).Build();
            var sut = Sut(productInsuranceCostService);

            var result=await sut.CalculateInsurance(insuranceRequest);

            result.OrderInsuranceValue.Should().Be(insurance * quantity);
            result.Contents.Count.Should().Be(1);
            result.Contents.First().ProductId.Should().Be(productId);
            result.Contents.First().Quantity.Should().Be(quantity);
        }

        [Fact]
        public async Task GivenMultiProductTypesInTheOrderWithQuantity_ReturnsCorrectInsuranceValue()
        {
            List<ProductInsuranceQuantity> productInsuranceQuantity = new()
            {
                ProductInsuranceQuantity.From(10, 0, 4),
                ProductInsuranceQuantity.From(11, 500, 3),
                ProductInsuranceQuantity.From(12, 1000, 2),
                ProductInsuranceQuantity.From(13, 2000, 1)
            };
            var productInsuranceCostService = FakeProductInsuranceCostService.Create();
            var orderInsuranceBuilder= OrderInsuranceRequestBuilder.Create();
            productInsuranceQuantity.ForEach(piq =>
                {
                    productInsuranceCostService.With(piq.ProductId, piq.Insurance);
                    orderInsuranceBuilder.With(piq.ProductId, piq.Quantity);
                });
            var insuranceRequest = orderInsuranceBuilder.Build();
            var sut = Sut(productInsuranceCostService);

            var result = await sut.CalculateInsurance(insuranceRequest);

            result.OrderInsuranceValue.Should().Be(5500);
            result.Contents.Count.Should().Be(4);
        }


        [Fact]
        public async Task GivenOrderContainingProductTypesInWithOrderLevel_ShouldAddExtraChargeOnTheTotalOrderInsurance()
        {
            List<ProductQuantity> productQuantities = new()
            {
                ProductQuantity.Create(ProductSamples.AppleIPod.Id, 4),
                ProductQuantity.Create(ProductSamples.SamsungGalaxyS10.Id, 2),
                ProductQuantity.Create(ProductSamples.AppleMacBookPro.Id, 2),
                ProductQuantity.Create(ProductSamples.SonyCyberShot.Id, 2)
            };
            var mockedProductDataApiClient = GetMockedProductDataApiClient();
            var productInsuranceCostService = new ProductInsuranceCostService(mockedProductDataApiClient, new List<IInsuranceStrategy>());
            var orderInsuranceBuilder = OrderInsuranceRequestBuilder.Create();
            productQuantities.ForEach(piq =>
            {
                orderInsuranceBuilder.With(piq.ProductId, piq.Quantity);
            });
            var insuranceRequest = orderInsuranceBuilder.Build();
            var sut = Sut(productInsuranceCostService, mockedProductDataApiClient);

            var result = await sut.CalculateInsurance(insuranceRequest);

            result.OrderInsuranceValue.Should().Be(500);
            result.Contents.Count.Should().Be(4);
        }

        private static IOrderInsuranceCalculatorService Sut(IProductInsuranceCostService productInsuranceCostService)
            => new OrderInsuranceCalculatorService(productInsuranceCostService,MockedInsuranceSurchargeRatesRepository.Create(500),
                FakeProductDataApiClient.Create());

        private static IOrderInsuranceCalculatorService Sut(IProductInsuranceCostService productInsuranceCostService,IProductDataApiClient productDataApiClient)
            => new OrderInsuranceCalculatorService(productInsuranceCostService, MockedInsuranceSurchargeRatesRepository.Create(500),
                productDataApiClient);

        private static IProductDataApiClient GetMockedProductDataApiClient()
        {
            var products=new Product[] 
            {
                ProductSamples.AppleIPod,
                ProductSamples.AppleMacBookPro,
                ProductSamples.SamsungGalaxyS10,
                ProductSamples.SonyCyberShot,
            };
            var productTypes = new ProductType[]
            {
                ProductTypeBuilder.Create().WithCanBeInsured(true).WithId(ProductTypeSamples.DigitalCameras).WithName(nameof(ProductTypeSamples.DigitalCameras)).Build(),
                ProductTypeBuilder.Create().WithCanBeInsured(true).WithId(ProductTypeSamples.Laptops).WithName(nameof(ProductTypeSamples.Laptops)).Build(),
                ProductTypeBuilder.Create().WithCanBeInsured(false).WithId(ProductTypeSamples.MP3Players).WithName(nameof(ProductTypeSamples.MP3Players)).Build(),
                ProductTypeBuilder.Create().WithCanBeInsured(true).WithId(ProductTypeSamples.Smartphones).WithName(nameof(ProductTypeSamples.Smartphones)).Build(),

            };

            return MockedProductDataApiBuilder.Create().WithProducts(products).WithProductTypes(productTypes).Build();
        }
        
        private class ProductInsuranceQuantity
        {
            private ProductInsuranceQuantity(int productId, double insurance, int quantity)
            {
                ProductId = productId;
                Insurance = insurance;
                Quantity = quantity;
            }

            public static ProductInsuranceQuantity From(int productId, double insurance, int quantity) =>
                new(productId, insurance, quantity);

            public int ProductId {  get; }
            public double Insurance {  get; }
            public int Quantity {  get; }
        }
    }
}
