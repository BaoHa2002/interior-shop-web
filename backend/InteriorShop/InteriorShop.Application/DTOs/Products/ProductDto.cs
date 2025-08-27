using InteriorShop.Application.DTOs.Option;

namespace InteriorShop.Application.DTOs.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string Sku { get; set; } = default!;

        public string? ShortDescription { get; set; }
        public string? SpectsHtml { get; set; }

        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int? Stock { get; set; }

        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }

        public int ViewCount { get; set; }
        public int SoldCount { get; set; }

        public DateTime? PublishedAt { get; set; }

        public List<string> ImageUrls { get; set; } = new();
        public List<ProductVariantDto> Variant { get; set; } = new();
        public List<OptionGroupDto> OptionGroups { get; set; } = new();
    }
}