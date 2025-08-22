using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string Url { get; set; } = default!;
        public bool IsPrimary { get; set; }
        public int SortOrder { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }
}
