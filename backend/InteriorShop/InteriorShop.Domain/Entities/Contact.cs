using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class Contact : BaseEntity
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }
        public string Message { get; set; } = default!;
    }
}