namespace InteriorShop.Application.Requests.Content
{
    public class BlogPostCreateRequest
    {
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Summary { get; set; }
        public string ContentHtml { get; set; } = default!;
        public string? ThumbnailUrl { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}