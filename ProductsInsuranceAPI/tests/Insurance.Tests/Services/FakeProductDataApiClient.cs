using System.Threading.Tasks;
using Insurance.DataProvider.Contract;
using Insurance.DataProvider.Model;

namespace Insurance.Tests.Services
{
    public class FakeProductDataApiClient : IProductDataApiClient
    {
        private Product _product { get; } = new() {Name = "ProdName",SalesPrice = 200 ,ProductTypeId = 10};
        private ProductType _productType { get; } = new() {Id = 10,Name = "ProductTypeName", CanBeInsured = true};


        public static FakeProductDataApiClient Create() => new ();

        public FakeProductDataApiClient WithProductSalePrice(double price)
        {
            _product.SalesPrice = price;
            return this;
        }
        public FakeProductDataApiClient WithProductTypeCanBeInsured(bool canBeInsured)
        {
            _productType.CanBeInsured=canBeInsured;
            return this;
        }

        public FakeProductDataApiClient WithProductTypeId(int productTypeId)
        {
            _productType.Id = productTypeId;
            _product.ProductTypeId = productTypeId;
            return this;
        }
        public FakeProductDataApiClient WithProductId(int productId)
        {
            _product.Id = productId;
            return this;
        }

        public Task<Product> GetProductById(int productId)
        {
            _product.Id = productId;
            return Task.FromResult(_product);
        }

        public Task<ProductType> GetProductTypeById(int productTypeId)
        {
            _productType.Id = productTypeId;
            return Task.FromResult(_productType);
        }

    }
}
