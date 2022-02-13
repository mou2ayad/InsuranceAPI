using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Insurance.Api.Contracts;
using Insurance.Api.Models;
using Insurance.DataProvider.Contract;

namespace Insurance.Api.Services
{
    public class OrderInsuranceCalculatorService : IOrderInsuranceCalculatorService
    {
        private readonly IProductInsuranceCostService _insuranceCostService;
        private readonly IInsuranceSurchargeRatesRepository _insuranceSurchargeRatesService;
        private readonly IProductDataApiClient _productDataApiClient;

        public OrderInsuranceCalculatorService(IProductInsuranceCostService insuranceCostService,
            IInsuranceSurchargeRatesRepository insuranceSurchargeRatesService, IProductDataApiClient productDataApiClient)
        {
            _insuranceCostService = insuranceCostService;
            _insuranceSurchargeRatesService = insuranceSurchargeRatesService;
            _productDataApiClient = productDataApiClient;
        }

        public async Task<OrderInsuranceResponse> CalculateInsurance(OrderInsuranceRequest orderProducts)
        {
            object locker = new object();
            double insuranceValue = 0;
            ConcurrentBag<int> productTypes = new();

            await CalculateInsuranceOnItemLevel();

            AddSurchargeOnOrderLevel();

            return OrderInsuranceResponse.From(orderProducts.Contents, insuranceValue);


            async Task CalculateInsuranceOnItemLevel()
            {
                await Task.WhenAll(orderProducts.Contents.Select(async pq =>
                {
                    var productTask = _productDataApiClient.GetProductById(pq.ProductId);
                    var productInsurance = await _insuranceCostService.CalculateInsurance(pq.ProductId);
                    if (productInsurance.InsuranceValue > 0)
                    {
                        lock (locker)
                        {
                            insuranceValue += productInsurance.InsuranceValue * pq.Quantity;
                        }
                    }
                    var product = await productTask;
                    productTypes.Add(product.ProductTypeId);
                }));
            }

            void AddSurchargeOnOrderLevel()
            {
                Parallel.ForEach(productTypes.Distinct(), prdTypeId =>
                {
                    var surchargeRates = _insuranceSurchargeRatesService.GetSurchargeRateOnOrderLevel(prdTypeId);
                    if (surchargeRates > 0)
                    {
                        lock (locker)
                        {
                            insuranceValue += surchargeRates;
                        }
                    }
                });
            }
        }
    }
}