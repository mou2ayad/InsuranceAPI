using Insurance.Api.Contracts;
using Insurance.DataProvider.Model;

namespace Insurance.Api.Services
{
    public class InsuranceProductTypeStrategy :IInsuranceStrategy
    {
        private readonly IInsuranceSurchargeRatesRepository _insuranceSurchargeRatesService;

        public InsuranceProductTypeStrategy(IInsuranceSurchargeRatesRepository insuranceSurchargeRatesService) 
            => _insuranceSurchargeRatesService = insuranceSurchargeRatesService;

        public double GetInsuranceValue(Product product) => 
            _insuranceSurchargeRatesService.GetSurchargeRateOnItemLevel(product.ProductTypeId);
    }
}
