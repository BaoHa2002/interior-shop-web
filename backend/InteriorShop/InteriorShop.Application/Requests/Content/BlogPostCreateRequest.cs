namespace InteriorShop.Application.Requests.Content
{
    public class BlogPostCreateRequest
    {
        public string Title { get; set; } = default!;
        public string? Slug { get; set; }
        public string? Excerpt { get; set; }
        public string ContentHtml { get; set; } = default!;
        public string? CoverImageUrl { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; } = false;

        // SEO
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }
}