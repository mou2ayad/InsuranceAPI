using System.Threading.Tasks;
using Insurance.Api.Models;

namespace Insurance.Api.Contracts
{
    public interface IProductInsuranceCostService
    {
        Task<ProductInsuranceResponse> CalculateInsurance(int productId);
    }
}