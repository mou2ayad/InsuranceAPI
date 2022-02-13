using System.Threading.Tasks;
using Insurance.DataProvider.Contract;
using Insurance.DataProvider.Model;
using Moq;

namespace Insurance.Tests.Services
{
    public class MockedProductDataApiBuilder
    {
        public static MockedProductDataApiBuilder Create() => new();

        private readonly Mock<IProductDataApiClient> _moqRepository = new();

        public MockedProductDataApiBuilder WithProducts(params Product[] products)
        {
            foreach (var product in products)
            {
                _moqRepository.Setup(e => e.GetProductById(product.Id)).Returns(Task.FromResult(product));
            }

            return this;
        }

        public MockedProductDataApiBuilder WithProductTypes(params ProductType[] productTypes)
        {
            foreach (var productType in productTypes)
            {
                _moqRepository.Setup(e => e.GetProductTypeById(productType.Id)).Returns(Task.FromResult(productType));
            }

            return this;
        }

        public IProductDataApiClient Build() => _moqRepository.Object;
    }
}