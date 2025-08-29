namespace InteriorShop.Application.Requests.Options
{
    // ===== OPTION GROUP =====
    public class OptionGroupCreateRequest
    {
        public string Name { get; set; } = default!;
        public bool IsGlobal { get; set; } = false;

        // nếu IsGlobal = false thì gắn cho 1 Product cụ thể
        public Guid? ProductId { get; set; }
    }

    public class OptionGroupUpdateRequest
    {
        public string Name { get; set; } = default!;
        public bool IsGlobal { get; set; } = false;
        public Guid? ProductId { get; set; }
    }

    // ===== OPTION VALUE =====
    public class OptionValueCreateRequest
    {
        public string Value { get; set; } = default!;
        public Guid OptionGroupId { get; set; }
    }

    public class OptionValueUpdateRequest
    {
        public string Value { get; set; } = default!;
    }
}
