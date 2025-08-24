using InteriorShop.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorShop.Domain.Entities
{
    public class SiteSetting : BaseEntity
    {
        public string SiteName { get; set; } = "PhatDecors";
        public string Hotline { get; set; } = default!;
        public string ShowroomAddress { get; set; } = default!;
        public decimal DefaultShippingFee { get; set; } = 0;

        // Branding
        public string? LogoUrl { get; set; }
        public string? FaviconUrl { get; set; }

        // Contact
        public string? ContactEmail { get; set; }

        // Bank transfer
        public string? BankAccountName { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }

        // SMTP
        public string? SmtpHost { get; set; }
        public int? SmtpPort { get; set; }
        public string? SmtpUser { get; set; }
        public string? SmtpPassword { get; set; }
        public string? SmtpFromEmail { get; set; }
    }
}
