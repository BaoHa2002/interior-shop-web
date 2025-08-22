using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;

        //Quan hệ cha/con
        public long? ParentId { get; set; }
        public Category? Parent { get; set; }
        public ICollection<Category> Children { get; set; } = new List<Category>();

        //Hiển thị UI/UX
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }

        //Quản trị
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        //Liên kết Product
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
