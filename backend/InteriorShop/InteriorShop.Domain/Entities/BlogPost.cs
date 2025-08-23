using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class BlogPost : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string Excerpt { get; set; } = default!; // mô tả ngắn hiển thị khi hover
        public string ContentHtml { get; set; } = default!;
        public string? CoverImageUrl { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; }

        // SEO (optional)
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}
