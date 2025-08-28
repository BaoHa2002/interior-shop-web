namespace InteriorShop.Application.Requests.Banners
{
    public class CampaignBannerCreateRequest
    {
        public string Title { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string? Link { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}