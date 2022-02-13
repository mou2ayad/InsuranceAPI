using System.Threading.Tasks;
using Insurance.Api.Models;

namespace Insurance.Api.Contracts
{
    public interface IOrderInsuranceCalculatorService
    {
        Task<OrderInsuranceResponse> CalculateInsurance(OrderInsuranceRequest orderProducts);
    }
}