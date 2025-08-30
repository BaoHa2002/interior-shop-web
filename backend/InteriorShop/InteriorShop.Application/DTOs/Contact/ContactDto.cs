namespace InteriorShop.Application.DTOs.Contact
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }
        public string Message { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}