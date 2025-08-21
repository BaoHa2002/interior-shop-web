using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorShop.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public long? ParentId { get; set; }
        public Category? Parent { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = true;

    }
}
