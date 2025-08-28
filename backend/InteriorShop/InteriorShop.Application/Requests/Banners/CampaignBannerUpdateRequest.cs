namespace InteriorShop.Application.Requests.Banners
{
    public class CampaignBannerUpdateRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string? Link { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}