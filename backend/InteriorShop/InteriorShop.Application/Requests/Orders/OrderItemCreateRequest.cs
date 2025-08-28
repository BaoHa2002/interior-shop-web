namespace InteriorShop.Application.Requests.Orders
{
    public class OrderItemCreateRequest
    {
        public Guid ProductId { get; set; }        // Sản phẩm
        public Guid? ProductVariantId { get; set; } // Nếu có variant (màu sắc, size...)
        public int Quantity { get; set; }           // Số lượng
        public decimal UnitPrice { get; set; }      // Giá tại thời điểm đặt
    }
}