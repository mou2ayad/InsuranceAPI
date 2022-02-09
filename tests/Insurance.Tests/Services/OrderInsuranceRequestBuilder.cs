using System.Collections.Generic;
using Insurance.Api.Models;

namespace Insurance.Tests.Services
{
    public class OrderInsuranceRequestBuilder
    {
        private OrderInsuranceRequest request;

        public OrderInsuranceRequestBuilder() =>
            request = new OrderInsuranceRequest
            {
                Contents = new List<ProductQuantity>()
            };

        public static OrderInsuranceRequestBuilder Create() => new();

        public OrderInsuranceRequestBuilder With(int productId, int quantity)
        {
            request.Contents.Add(new() {Quantity = quantity, ProductId = productId});
            return this;
        }
        public OrderInsuranceRequest Build() => request;

    }
}
