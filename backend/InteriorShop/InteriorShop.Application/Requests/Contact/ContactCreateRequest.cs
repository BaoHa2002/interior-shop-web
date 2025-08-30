namespace InteriorShop.Application.Requests.Contact
{
    public class ContactCreateRequest
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }
        public string Message { get; set; } = default!;
    }
}