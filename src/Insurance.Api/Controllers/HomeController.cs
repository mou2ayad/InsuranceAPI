using System.Threading.Tasks;
using Insurance.Api.Contracts;
using Insurance.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    public class HomeController: Controller
    {
      
        [HttpGet]
        [Route("api/insurance/product")]
        public async Task<ProductInsuranceResponse> CalculateProductInsurance(
            [FromServices] IProductInsuranceCostService insuranceCostService, int productId) 
            => await insuranceCostService.CalculateInsurance(productId);

        [HttpPost]
        [Route("api/insurance/order")]
        public async Task<OrderInsuranceResponse> CalculateOrderInsurance(
            [FromServices] IOrderInsuranceCalculatorService orderInsuranceCalculatorService,[FromBody] OrderInsuranceRequest request) 
            => await orderInsuranceCalculatorService.CalculateInsurance(request);
    }
}