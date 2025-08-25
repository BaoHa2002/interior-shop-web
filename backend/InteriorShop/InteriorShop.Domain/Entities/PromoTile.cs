using InteriorShop.Domain.Common;

namespace InteriorShop.Domain.Entities
{
    public class PromoTile : BaseEntity
    {
        public string ImageUrl { get; set; } = default!;
        public string? AltText { get; set; }
        public string? Title { get; set; }      // dòng to
        public string? Subtitle { get; set; }   // dòng nhỏ
        public string? TextColor { get; set; }  // #ffffff hoặc class Tailwind
        public string Url { get; set; } = default!;
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }
}
