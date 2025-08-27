namespace InteriorShop.Application.DTOs.Products
{
    public class ProductVariantDto
    {
        public Guid Id { get; set; }
        public string Sku { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int Stock { get; set; }
        public string OptionsJson { get; set; } = "{}";
    }
}