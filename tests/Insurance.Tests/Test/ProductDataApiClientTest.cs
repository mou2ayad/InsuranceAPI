using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Insurance.DataProvider.Service;
using Insurance.Tests.Utils;
using Insurance.Utilities.ErrorHandling;
using Xunit;

namespace Insurance.Tests.Test
{
    public class ProductDataApiClientTest : IClassFixture<FakeProductApiFixture>
    {
        private readonly FakeProductApiFixture _fixture;

        public ProductDataApiClientTest(FakeProductApiFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ProductDataApiClient_GivenValidProductId_ShouldReturnProductModel()
        {
            var sut = Sut();

            var response=await sut.GetProductById(10);

            response.Should().NotBeNull();
            response.Id.Should().Be(10);
        }

        [Fact]
        public async Task ProductDataApiClient_GivenInvalidProductId_ShouldReturn404IncludingTraceId_WithoutRetry()
        {
            var logger = FakeLogger<ProductDataApiClient>.Create();
            var sut = Sut(logger);
            int invalidProductId = 404;

            Func<Task> act = () => sut.GetProductById(invalidProductId);
            
            await act.Should().ThrowAsync<HttpRequestException>().WithMessage("Fail in http call with status [NotFound], traceId: [1234]");
            logger.NumberOfLogs.Should().Be(0);
        }

        [Fact]
        public async Task ProductDataApiClient_GivenInvalidProductId_ShouldReturnConnectivityException_AndRetry5Times()
        {
            var logger = FakeLogger<ProductDataApiClient>.Create();
            var sut = Sut(logger);
            int productIdReturnsBadRequest = 502;

            Func<Task> act = () => sut.GetProductById(productIdReturnsBadRequest);

            await act.Should().ThrowAsync<ConnectivityException>().WithMessage("Connectivity error, status code: 502");
            logger.NumberOfLogs.Should().Be(5); // 5 retries and 1 error 
        }

        
        private ProductDataApiClient Sut()
        {
            var httpClient = new HttpClient{ BaseAddress = new Uri(_fixture.BaseUrl)};
            return new ProductDataApiClient(httpClient, FakeLogger<ProductDataApiClient>.Create());
        }
        private ProductDataApiClient Sut(FakeLogger<ProductDataApiClient> logger)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(_fixture.BaseUrl) };
            return new ProductDataApiClient(httpClient, logger);
        }
    }
}
