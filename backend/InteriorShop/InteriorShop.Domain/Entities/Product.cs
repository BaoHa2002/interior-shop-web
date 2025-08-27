using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class Product : BaseEntity
    {
        //Cơ bản
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string Sku { get; set; } = default!;

        //Mô tả
        public string? ShortDescription { get; set; }
        public string? SpectsHtml { get; set; }

        // Giá & kho
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int? Stock { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        // SEO
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        // Marketing & tracking
        public int ViewCount { get; set; } = 0;
        public int SoldCount { get; set; } = 0;
        public DateTime? PublishedAt { get; set; }

        //Quan hệ
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<OptionGroup> OptionsGroups { get; set; } = new List<OptionGroup>();
    }
}
