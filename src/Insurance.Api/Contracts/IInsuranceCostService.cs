using System.Threading.Tasks;
using Insurance.Api.Models;

namespace Insurance.Api.Contracts
{
    public interface IInsuranceCostService
    {
        Task<ProductInsuranceResponse> CalculateInsurance(int productId);
    }
}