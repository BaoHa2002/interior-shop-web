namespace InteriorShop.Application.DTOs.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }

        public Guid? ParentId { get; set; }
        public string? ParentName { get; set; }

        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}