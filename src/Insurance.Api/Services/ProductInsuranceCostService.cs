using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Contracts;
using Insurance.Api.Models;
using Insurance.DataProvider.Contract;
using Insurance.Utilities.ErrorHandling;

namespace Insurance.Api.Services
{
    public class ProductInsuranceCostService : IProductInsuranceCostService
    {
        private readonly IProductDataApiClient _productDataApiClient;
        private readonly IEnumerable<IInsuranceStrategy> _insuranceStrategies;

        public ProductInsuranceCostService(IProductDataApiClient productDataApiClient,
            IEnumerable<IInsuranceStrategy> insuranceStrategies)
        {
            _productDataApiClient = productDataApiClient;
            _insuranceStrategies = insuranceStrategies;
        }

        public async Task<ProductInsuranceResponse> CalculateInsurance(int productId)
        {
            var product = await _productDataApiClient.GetProductById(productId);
            if (product == null || product.Id == 0)
                throw new ClientException($"invalid productId [{productId}]");
            var productType = await _productDataApiClient.GetProductTypeById(product.ProductTypeId);

            double insurance = 0;
            if (productType.CanBeInsured)
                foreach (var insuranceRulesService in _insuranceStrategies)
                    insurance += insuranceRulesService.GetInsuranceValue(product);

            return ProductInsuranceResponse.From(productId,insurance);
        }
    }
}
