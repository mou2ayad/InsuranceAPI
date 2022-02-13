using Insurance.DataProvider.Model;

namespace Insurance.Api.Contracts
{
    public interface IInsuranceStrategy
    {
        double GetInsuranceValue(Product product);
    }
}