using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string FullName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string AddressLine { get; set; } = default!;
        public string Ward { get; set; } = default!;
        public string District { get; set; } = default!;
        public string Province { get; set; } = default!;
        public string? Email { get; set; }
        public string? Note { get; set; }
        public bool IsDefault { get; set; } = false;
        public Guid? UserId { get; set; }
    }
}
