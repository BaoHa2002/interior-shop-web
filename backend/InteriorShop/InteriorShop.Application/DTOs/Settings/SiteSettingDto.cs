namespace InteriorShop.Application.DTOs.Settings
{
    public class SiteSettingDto
    {
        public Guid Id { get; set; }
        public string SiteName { get; set; } = default!;
        public string? LogoUrl { get; set; }
        public string ContactEmail { get; set; } = default!;
        public string ContactPhone { get; set; } = default!;
        public string Address { get; set; } = default!;
        public decimal DefaultShippingFee { get; set; }
    }
}