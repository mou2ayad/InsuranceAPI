using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Insurance.Api.Contracts;
using Insurance.Api.Models;

namespace Insurance.Api.Services
{
    public class OrderInsuranceCalculatorService : IOrderInsuranceCalculatorService
    {
        private readonly IProductInsuranceCostService _insuranceCostService;

        public OrderInsuranceCalculatorService(IProductInsuranceCostService insuranceCostService)
        {
            _insuranceCostService = insuranceCostService;
        }

        public async Task<OrderInsuranceResponse> CalculateInsurance(OrderInsuranceRequest orderProducts)
        {
            object locker = new object();
            double insuranceValue = 0;

            await Task.WhenAll(orderProducts.Contents.Select(async pq =>
            {
                var productInsurance = await _insuranceCostService.CalculateInsurance(pq.ProductId);
                lock (locker)
                {
                    insuranceValue += productInsurance.InsuranceValue * pq.Quantity;
                }
            }));
            return OrderInsuranceResponse.From(orderProducts.Contents, insuranceValue);
        }
    }
}