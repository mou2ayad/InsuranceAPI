using System.Collections.Generic;

namespace Insurance.Api.Models
{
    public class OrderInsuranceRequest
    {
        public List<ProductQuantity> Contents { set; get; }
    }
}
