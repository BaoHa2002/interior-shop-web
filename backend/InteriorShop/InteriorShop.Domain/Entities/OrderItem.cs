using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;

        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string? ProductSlug { get; set; }

        public string? VariantSku { get; set; }
        public string? SelectedOptionsJson { get; set; } // {"Color":"Black","Size":"L"}

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; } // lưu lại để khóa dữ liệu giao dịch
    }
}
