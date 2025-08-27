namespace InteriorShop.Application.DTOs.Option
{
    public class OptionGroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public bool IsGlobal { get; set; } = false;

        public Guid? ProductId { get; set; }
        public List<OptionValueDto> Values { get; set; } = new();
    }
}