namespace Insurance.Api.Models
{
    public class ProductQuantity
    {
        public static ProductQuantity Create(int productId, int quantity)
            => new() {ProductId = productId, Quantity = quantity};
        
        public int ProductId { set; get; }
        public int Quantity { set; get; }
    }
}
