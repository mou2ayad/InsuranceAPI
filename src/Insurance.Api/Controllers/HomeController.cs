using System.Threading.Tasks;
using Insurance.Api.Contracts;
using Insurance.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    public class HomeController: Controller
    {
        private readonly IInsuranceCostService _insuranceCostService;

        public HomeController(IInsuranceCostService insuranceCostService)
        {
            _insuranceCostService = insuranceCostService;
        }

        [HttpGet]
        [Route("api/insurance/product")]
        public async Task<ProductInsuranceResponse> CalculateInsurance(int productId) =>
            await _insuranceCostService.CalculateInsurance(productId);
    }
}