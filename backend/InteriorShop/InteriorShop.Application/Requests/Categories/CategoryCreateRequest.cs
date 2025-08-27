namespace InteriorShop.Application.Requests.Categories
{
    public class CategoryCreateRequest
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public Guid? ParentId { get; set; }
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
    }
}