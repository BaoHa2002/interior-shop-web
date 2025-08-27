namespace InteriorShop.Application.DTOs.Orders
{
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string? ProductSlug { get; set; }
        public string? VariantSku { get; set; }
        public string? SelectedOptionsJson { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}