namespace InteriorShop.Application.DTOs.Products
{
    public class CartProductDto
    {
        public Guid ProductId { get; set; }
        public Guid? ProductVariantId { get; set; }
        public string Name { get; set; } = default!;
        public string? VariantSku { get; set; }
        public string? OptionsText { get; set; } // đọc từ OptionsJson để hiển thị
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
        public string? ThumbnailUrl { get; set; }
        public bool IsInStock { get; set; }
    }
}