namespace InteriorShop.Application.DTOs.Option
{
    public class OptionValueDto
    {
        public Guid Id { get; set; }
        public string Value { get; set; } = default!;
        public Guid OptionGroupId { get; set; }
    }
}