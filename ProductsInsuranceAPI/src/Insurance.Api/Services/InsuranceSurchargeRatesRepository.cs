using System.Linq;
using Insurance.Api.Contracts;
using Insurance.Storage;

namespace Insurance.Api.Services
{
    public class InsuranceSurchargeRatesRepository : IInsuranceSurchargeRatesRepository
    {
        public double GetSurchargeRateOnItemLevel(int productTypeId)=> GetSurchargeRateValue(productTypeId, "item");

        public double GetSurchargeRateOnOrderLevel(int productTypeId) => GetSurchargeRateValue(productTypeId, "order");
        
        private double GetSurchargeRateValue(int productTypeId,string chargingLevel)
        {
            var surchargeRate =
                InsuranceSurchargeRatesDb.Db.FirstOrDefault(e =>
                    e.ChargingLevel == chargingLevel && e.ProductTypeId == productTypeId);
            return surchargeRate?.InsuranceValue ?? 0;
        }
    }
}
