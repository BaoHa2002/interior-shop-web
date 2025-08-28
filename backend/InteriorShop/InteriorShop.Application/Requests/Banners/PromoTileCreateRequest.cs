namespace InteriorShop.Application.Requests.Banners
{
    public class PromoTileCreateRequest
    {
        public string Title { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string? Link { get; set; }
        public int SortOrder { get; set; } = 0;
    }
}