using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class CampaignBanner : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string? Subtitle { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public string ImageUrl { get; set; } = default!;
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
