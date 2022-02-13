using System.Collections.Generic;

namespace Insurance.Api.Models
{
    public class OrderInsuranceResponse
    {
        public OrderInsuranceResponse(List<ProductQuantity> contents, double insuranceValue)
        {
            Contents = contents;
            OrderInsuranceValue = insuranceValue;
        }

        public static OrderInsuranceResponse From(List<ProductQuantity> contents, double insuranceValue)
            => new (contents, insuranceValue);
        public double OrderInsuranceValue { set; get; }

        public List<ProductQuantity> Contents { set; get; }
    }
}
