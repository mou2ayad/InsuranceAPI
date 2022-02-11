using Insurance.DataProvider.Model;

namespace Insurance.Tests.Builders
{
    public class ProductTypeBuilder
    {
        private readonly ProductType _productType = new() {Id = 1, Name = "anyType", CanBeInsured = false};
        public static ProductTypeBuilder Create() => new();

        public ProductTypeBuilder WithId(int productTypeId)
        {
            _productType.Id = productTypeId;
            return this;
        }
        public ProductTypeBuilder WithCanBeInsured(bool canBeInsured)
        {
            _productType.CanBeInsured = canBeInsured;
            return this;
        }
        public ProductTypeBuilder WithName(string name)
        {
            _productType.Name = name;
            return this;
        }

        public ProductType Build() => _productType;

    }
}
