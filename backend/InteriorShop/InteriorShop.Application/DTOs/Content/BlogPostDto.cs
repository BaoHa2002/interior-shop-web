namespace InteriorShop.Application.DTOs.Content
{
    public class BlogPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Excerpt { get; set; }   // khớp với Domain
        public string ContentHtml { get; set; } = default!;
        public string? CoverImageUrl { get; set; } // khớp với Domain
        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; }

        // SEO
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}