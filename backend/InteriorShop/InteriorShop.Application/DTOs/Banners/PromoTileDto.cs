namespace InteriorShop.Application.DTOs.Banners
{
    public class PromoTileDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string? Link { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}