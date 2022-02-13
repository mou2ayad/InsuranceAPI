using System.Collections.Generic;
using Insurance.Api.Models;

namespace Insurance.Tests.Builders
{
    public class OrderInsuranceRequestBuilder
    {
        private readonly OrderInsuranceRequest _request;

        public OrderInsuranceRequestBuilder() =>
            _request = new OrderInsuranceRequest
            {
                Contents = new List<ProductQuantity>()
            };

        public static OrderInsuranceRequestBuilder Create() => new();

        public OrderInsuranceRequestBuilder With(int productId, int quantity)
        {
            _request.Contents.Add(new() {Quantity = quantity, ProductId = productId});
            return this;
        }
        public OrderInsuranceRequest Build() => _request;

    }
}
