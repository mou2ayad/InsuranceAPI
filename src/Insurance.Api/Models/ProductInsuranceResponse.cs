namespace Insurance.Api.Models
{
    public class ProductInsuranceResponse
    {
        public ProductInsuranceResponse(int productId, double insuranceValue)
        {
            ProductId = productId;
            InsuranceValue = insuranceValue;
        }

        public static ProductInsuranceResponse From(int productId, double insuranceValue) => 
            new (productId, insuranceValue);

        public int ProductId { get; set; }
        public double InsuranceValue { get; set; }
    }
}
