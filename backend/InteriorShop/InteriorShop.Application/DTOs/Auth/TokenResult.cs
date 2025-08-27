namespace InteriorShop.Application.DTOs.Auth
{
    public class TokenResult
    {
        public string AccessToken { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string? RefeshToken { get; set; }
    }
}
