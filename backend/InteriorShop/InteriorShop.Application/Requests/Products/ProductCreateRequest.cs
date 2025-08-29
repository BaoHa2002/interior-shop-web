namespace InteriorShop.Application.Requests.Products
{
    public class ProductCreateRequest
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string Sku { get; set; } = default!;

        public string? ShortDescription { get; set; }
        public string? SpectsHtml { get; set; }

        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int? Stock { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        // Quan hệ
        public List<Guid>? CategoryIds { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}