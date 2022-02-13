using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Contracts;
using Insurance.Api.Models;

namespace Insurance.Tests.Services
{
    public class FakeProductInsuranceCostService : IProductInsuranceCostService
    {
        private readonly Dictionary<int, double> _productInsurance = new();

        public static FakeProductInsuranceCostService Create() => new();

        public FakeProductInsuranceCostService With(int product, double insurance)
        {
            _productInsurance.Add(product, insurance);
            return this;
        }

        public Task<ProductInsuranceResponse> CalculateInsurance(int productId) =>
            Task.FromResult(ProductInsuranceResponse.From(productId, _productInsurance[productId]));

    }
}
