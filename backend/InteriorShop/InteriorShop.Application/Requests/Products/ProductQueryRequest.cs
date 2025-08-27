namespace InteriorShop.Application.Requests.Products
{
    public class ProductQueryRequest
    {
        // Paging
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;

        // Filters
        public string? Keyword { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsFeatured { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Sort: name_asc, name_desc, price_asc, price_desc, newest
        public string? SortBy { get; set; }
    }
}