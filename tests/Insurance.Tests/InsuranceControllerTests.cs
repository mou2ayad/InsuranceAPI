using System;
using System.Net.Http;
using System.Threading.Tasks;
using Insurance.Api.Contracts;
using Insurance.Api.Controllers;
using Insurance.Api.Services;
using Insurance.DataProvider.Service;
using Insurance.Tests.Services;
using Insurance.Tests.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace Insurance.Tests
{
    public class InsuranceControllerTests : IClassFixture<InsuranceControllerTests.ControllerTestFixture>
    {
        private readonly ControllerTestFixture _fixture;

        public InsuranceControllerTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            double expectedInsuranceValue = 1000;
            var insuranceCostService = CreateInsuranceCostService();
            var productId = 1;

            var sut = new HomeController();

            var response = await sut.CalculateProductInsurance(insuranceCostService,productId);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: response.InsuranceValue
            );
        }

        [Fact]
        public async Task CalculateInsuranceForOrder_GivenOneProductTypeSalesPriceBetween500And2000Euros_ShouldAddTwoThousandEurosToInsuranceCost()
        {
            double expectedInsuranceValue = 2000;
            var OrderInsuranceCostService = CreateOrderInsuranceCalculatorService();
            var productId = 1;

            var sut = new HomeController();
            var orderInsuranceRequest = OrderInsuranceRequestBuilder.Create().With(productId, 2).Build();
            var response = await sut.CalculateOrderInsurance(OrderInsuranceCostService, orderInsuranceRequest);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: response.InsuranceValue
            );
        }

        private IProductInsuranceCostService CreateInsuranceCostService()
        {
            var httpClient = new HttpClient {BaseAddress = new Uri(_fixture.BaseUrl)};
            var productDataApiClient = new ProductDataApiClient(httpClient, FakeLogger<ProductDataApiClient>.Create());
            return new ProductInsuranceCostService(productDataApiClient, FakeLogger<ProductInsuranceCostService>.Create());

        }

        private IOrderInsuranceCalculatorService CreateOrderInsuranceCalculatorService() => 
            new OrderInsuranceCalculatorService(CreateInsuranceCostService());

        public class ControllerTestFixture : IDisposable
        {
            private readonly IHost _host;
            public readonly string BaseUrl = "http://localhost:5005";

            public ControllerTestFixture()
            {
                _host = new HostBuilder()
                    .ConfigureWebHostDefaults(
                        b => b.UseUrls(BaseUrl)
                            .UseStartup<ControllerTestStartup>()
                    )
                    .Build();

                _host.Start();
            }

            public void Dispose() => _host.Dispose();
        }

        public class ControllerTestStartup
        {
            public void Configure(IApplicationBuilder app)
            {
                app.UseRouting();
                app.UseEndpoints(
                    ep =>
                    {
                        ep.MapGet(
                            "products/{id:int}",
                            context =>
                            {
                                int productId = int.Parse(context.Request.RouteValues["id"].ToString());
                                var product = new
                                {
                                    id = productId,
                                    name = "Test Product",
                                    productTypeId = 1,
                                    salesPrice = 750
                                };
                                return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                            }
                        );
                        ep.MapGet(
                            "product_types/{id:int}",
                            context =>
                            {
                                int productTypeId = int.Parse(context.Request.RouteValues["id"].ToString());
                                var product = new
                                {
                                    id = productTypeId,
                                    name = "Test Product Type",
                                    CanBeInsured = true
                                };
                                return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                            }
                        );
                        ep.MapGet(
                            "product_types",
                            context =>
                            {
                                var productTypes = new[]
                                {
                                    new
                                    {
                                        id = 1,
                                        name = "Test type",
                                        canBeInsured = true
                                    }
                                };
                                return context.Response.WriteAsync(JsonConvert.SerializeObject(productTypes));
                            }
                        );
                    }
                );
            }
        }
    }
}