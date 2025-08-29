namespace InteriorShop.Application.Requests.Settings
{
    public class UpdateSiteSettingRequest
    {
        public string SiteName { get; set; } = default!;
        public string Hotline { get; set; } = default!;
        public string ShowroomAddress { get; set; } = default!;
        public decimal DefaultShippingFee { get; set; }

        public string? LogoUrl { get; set; }
        public string? FaviconUrl { get; set; }

        public string? ContactEmail { get; set; }

        public string? BankAccountName { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }

        public string? SmtpHost { get; set; }
        public int? SmtpPort { get; set; }
        public string? SmtpUser { get; set; }
        public string? SmtpPassword { get; set; }
        public string? SmtpFromEmail { get; set; }
    }
}