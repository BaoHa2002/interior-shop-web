namespace InteriorShop.Application.Requests.Auth
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; } = default!;
    }
}