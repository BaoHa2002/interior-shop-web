using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class OptionValue : BaseEntity
    {
        public string Value { get; set; } = default!;
        public Guid OptionGroupId { get; set; }
        public OptionGroup OptionGroup { get; set; } = default!;
    }
}
