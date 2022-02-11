using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Insurance.Api.Contracts;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Insurance.DataProvider.Contract;
using Insurance.Tests.Builders;
using Insurance.Tests.Services;
using Insurance.Tests.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace Insurance.Tests.Test
{
    public class InsuranceCostServiceDecoratorTest
    {
        [Fact]
        public async Task CacheProductInsurance_afterGettingFromSource_ifNotExistInCache()
        {
            int salePrice = 800;
            double expectedInsurance = 1000;
            int productId = 1;
            int cacheExpireAfterInMinutes = 5;
            var productDataClient = CreateProductDataClient()
                .WithProductSalePrice(salePrice)
                .WithProductTypeCanBeInsured(true);
            var cache = FakeCache.Create();
            var logger = CreateLogger();
            var sut = Sut(productDataClient,cache, cacheExpireAfterInMinutes, logger);
            
            var productInsuranceResponse = await sut.CalculateInsurance(productId);

            productInsuranceResponse.InsuranceValue.Should().Be(expectedInsurance);
            string cacheKey = GetCacheKey(productId);
            var valueFromCache = await cache.Get<ProductInsuranceResponse>(cacheKey);

            cache.Key.Should().Be(cacheKey);
            valueFromCache.Should().BeEquivalentTo(productInsuranceResponse);
            cache.Expiration.Should().Be(TimeSpan.FromMinutes(cacheExpireAfterInMinutes));
            logger.AddedLogLevel.Should().Be(LogLevel.Information);
            logger.LoggedMessage.Should().StartWith($"Product [{productId}] is not exist");
        }

        [Fact]
        public async Task GetProductInsuranceFromCache_WithoutCallingProductAPI_ifExistsInCache()
        {
            double expectedInsurance = 1000;
            int productId = 1;
            var cacheKey = GetCacheKey(productId);
            var cachedProductInsuranceResponse = ProductInsuranceResponse.From(productId, expectedInsurance);
            var cache = FakeCache.Create().With(cacheKey, cachedProductInsuranceResponse, 5);
            var logger = CreateLogger();
            var sut = Sut(cache, logger);

            var productInsuranceResponse = await sut.CalculateInsurance(productId);


            cache.Key.Should().Be(cacheKey);
            productInsuranceResponse.Should().BeEquivalentTo(cachedProductInsuranceResponse);
            logger.IsLogAdded.Should().BeFalse();
            logger.LoggedMessage.Should().BeNull();
        }



        private FakeProductDataApiClient CreateProductDataClient() =>
            FakeProductDataApiClient.Create();

        private IProductInsuranceCostService Sut(IProductDataApiClient productDataApiClient,FakeCache cache, int cacheExpireAfterInMinutes,FakeLogger<ProductInsuranceCostServiceCacheDecorator> logger)
        {
            var insurance = new ProductInsuranceCostService(productDataApiClient,InsuranceStrategiesHelper.GetStrategies());
            return new ProductInsuranceCostServiceCacheDecorator(CreateOptions(cacheExpireAfterInMinutes), cache, insurance,logger);
        }

        private IProductInsuranceCostService Sut(FakeCache cache, FakeLogger<ProductInsuranceCostServiceCacheDecorator> logger)
            => Sut(CreateProductDataClient(), cache, 5, logger);

        private IOptions<InsuranceCostServiceConfig> CreateOptions(int expireAfterInMinutes) =>
            Options.Create(new InsuranceCostServiceConfig {EnableCaching = true,ExpireAfterInMinutes = expireAfterInMinutes });

        private string GetCacheKey(int productId) => $"PRD_INC_{productId}";

        private FakeLogger<ProductInsuranceCostServiceCacheDecorator> CreateLogger() =>
            FakeLogger<ProductInsuranceCostServiceCacheDecorator>.Create();
        
    }
}
