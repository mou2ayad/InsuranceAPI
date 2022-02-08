using System.Threading.Tasks;
using Insurance.Api.Contracts;
using Insurance.Api.Models;
using Insurance.DataProvider.Contract;
using Insurance.Utilities.ErrorHandling;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Services
{
    public class InsuranceCostService : IInsuranceCostService
    {
        private readonly IProductDataApiClient _productDataApiClient;
        private ILogger<InsuranceCostService> _logger;

        public InsuranceCostService(IProductDataApiClient productDataApiClient, ILogger<InsuranceCostService> logger)
        {
            _productDataApiClient = productDataApiClient;
            _logger = logger;
        }

        public async Task<ProductInsuranceResponse> CalculateInsurance(int productId)
        {
            var product = await _productDataApiClient.GetProductById(productId);
            if (product == null || product.Id == 0)
                throw new ClientException($"invalid productId [{productId}]");
            var productType = await _productDataApiClient.GetProductTypeById(product.ProductTypeId);

            float insurance = 0;
            if (productType.CanBeInsured)
            {
                if (product.SalesPrice is >= 500 and < 2000)
                    insurance = 1000;
                else if (product.SalesPrice >= 2000)
                        insurance = 2000;
                if (productType.Name == "Laptops" || productType.Name == "Smartphones")
                    insurance += 500;
            }

            return ProductInsuranceResponse.From(productId,insurance);
        }
    }
}
