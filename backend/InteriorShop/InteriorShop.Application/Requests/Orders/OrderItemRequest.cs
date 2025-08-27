namespace InteriorShop.Application.Requests.Orders
{
    public class OrderItemRequest
    {
        public Guid ProductId { get; set; }
        public Guid? ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}