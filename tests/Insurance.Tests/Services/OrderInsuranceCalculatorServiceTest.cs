using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Insurance.Api.Contracts;
using Insurance.Api.Services;
using Xunit;

namespace Insurance.Tests.Services
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

            result.InsuranceValue.Should().Be(insurance * quantity);
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

            result.InsuranceValue.Should().Be(5500);
            result.Contents.Count.Should().Be(4);
        }

        private static IOrderInsuranceCalculatorService Sut(IProductInsuranceCostService productInsuranceCostService)
            => new OrderInsuranceCalculatorService(productInsuranceCostService);

        private class ProductInsuranceQuantity
        {
            public ProductInsuranceQuantity(int productId, double insurance, int quantity)
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
