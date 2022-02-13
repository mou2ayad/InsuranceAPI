using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Insurance.DataProvider.Configuration;
using Insurance.DataProvider.Contract;
using Insurance.DataProvider.Model;
using Insurance.Utilities.Cache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Insurance.DataProvider.Service
{
    public class ProductDataApiClientCacheDecorator :IProductDataApiClient
    {
        private readonly IProductDataApiClient _productDataApiClient;
        private readonly ILogger<ProductDataApiClientCacheDecorator> _logger;
        private readonly ICache _cache;
        private readonly ProductApiConfig _config;
        public ProductDataApiClientCacheDecorator(IProductDataApiClient productDataApiClient, ILogger<ProductDataApiClientCacheDecorator> logger, ICache cache,IOptions<ProductApiConfig> config)
        {
            _productDataApiClient = productDataApiClient;
            _logger = logger;
            _cache = cache;
            _config = config.Value;
        }
        public async Task<Product> GetProductById(int productId)
        {
            var cacheKey = GetProductCacheKey(productId);
            var product=await _cache.Get<Product>(cacheKey);
            if (product == null)
            {
                product=await _productDataApiClient.GetProductById(productId);
                await _cache.Set(cacheKey, product, TimeSpan.FromMinutes(_config.ExpireAfterInMinutes));
                _logger.LogInformation($"Product [{productId}] is not available in Cache, getting from the source and adding to cache");
            }
            return product;
        }

        public async Task<ProductType> GetProductTypeById(int productTypeId)
        {
            var cacheKey = GetProductTypeCacheKey(productTypeId);
            var productType = await _cache.Get<ProductType>(cacheKey);
            if (productType == null)
            {
                productType = await _productDataApiClient.GetProductTypeById(productTypeId);
                await _cache.Set(cacheKey, productType, TimeSpan.FromMinutes(_config.ExpireAfterInMinutes));
                _logger.LogInformation($"ProductType [{productTypeId}] is not available in Cache, getting from the source and adding to cache");
            }
            return productType;
        }

        private static string GetProductTypeCacheKey(int productTypeId) => $"PT_{productTypeId}";

        private static string GetProductCacheKey(int productId) => $"P_{productId}";
    }
}
