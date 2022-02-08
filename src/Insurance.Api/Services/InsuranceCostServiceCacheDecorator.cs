using System;
using System.Threading.Tasks;
using Insurance.Api.Contracts;
using Insurance.Api.Models;
using Insurance.Utilities.Cache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Insurance.Api.Services
{
    public class InsuranceCostServiceCacheDecorator : IInsuranceCostService
    {
        private readonly ICache _cache;
        private readonly InsuranceCostServiceConfig _config;
        private readonly IInsuranceCostService _insuranceCostService;
        private readonly ILogger<InsuranceCostServiceCacheDecorator> _logger;

        public InsuranceCostServiceCacheDecorator(IOptions<InsuranceCostServiceConfig> options, ICache cache,
            IInsuranceCostService insuranceCostService, ILogger<InsuranceCostServiceCacheDecorator> logger)
        {
            _config = options.Value;
            _cache = cache;
            _insuranceCostService = insuranceCostService;
            _logger = logger;
        }

        public async Task<ProductInsuranceResponse> CalculateInsurance(int productId)
        {
            var key = GetInsuranceCostKey(productId);
            var insurance = await _cache.Get<ProductInsuranceResponse>(key);
            if (insurance == null)
            {
                _logger.LogInformation($"Product [{productId}] is not exist in Cache, getting the insurance value from the source");
                insurance = await _insuranceCostService.CalculateInsurance(productId);
                await _cache.Set(key, insurance, TimeSpan.FromMinutes(_config.ExpireAfterInMinutes));
            }

            return await Task.FromResult(insurance);
        }
        private string GetInsuranceCostKey(int productId) => $"PRD_INC_{productId}";

    }
}
