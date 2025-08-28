namespace InteriorShop.Application.Requests.Users
{
    public class UserUpdateRequest
    {
        public string FullName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public string? AddressLine { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
    }
}