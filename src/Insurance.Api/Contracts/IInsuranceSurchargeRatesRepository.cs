namespace Insurance.Api.Contracts
{
    public interface IInsuranceSurchargeRatesRepository
    {
        double GetSurchargeRateOnItemLevel(int productTypeId);
        double GetSurchargeRateOnOrderLevel(int productTypeId);
    }
}