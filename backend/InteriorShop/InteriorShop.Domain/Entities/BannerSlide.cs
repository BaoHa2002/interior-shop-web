using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class BannerSlide : BaseEntity
    {
        public string ImageUrl { get; set; } = default!;
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? CtaText { get; set; }
        public string? CtaUrl { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
