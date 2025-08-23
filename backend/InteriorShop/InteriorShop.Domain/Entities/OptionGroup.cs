using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class OptionGroup : BaseEntity
    {
        public string Name { get; set; } = default!;
        public bool IsGlobal { get; set; } = false; // true = dùng chung nhiều product, false = riêng cho product
        public ICollection<OptionValue> Values { get; set; } = new List<OptionValue>();

        //Nếu IsGlobal = false thì mới cần ProductId
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
