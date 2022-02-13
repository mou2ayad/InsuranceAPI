using Insurance.DataProvider.Model;

namespace Insurance.Tests.Builders
{
    public class ProductBuilder
    {
        private readonly Product _product = new() {Id = 1, Name = "anyProduct", ProductTypeId = 100, SalesPrice = 400};
        public static ProductBuilder Create() => new();

        public ProductBuilder WithProductTypeId(int productTypeId)
        {
            _product.ProductTypeId = productTypeId;
            return this;
        }

        public ProductBuilder WithId(int productId)
        {
            _product.Id = productId;
            return this;
        }
        public ProductBuilder WithPrice(double price)
        {
            _product.SalesPrice = price;
            return this;
        }
        public ProductBuilder WithName(string name)
        {
            _product.Name = name;
            return this;
        }

        public Product Build() => _product;

    }
}
