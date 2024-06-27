namespace OnlineEducationWebApp.Models
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime AccessTokenExpireTime { get; set; }

        public DateTime? RefreshTokenExpireTime { get; set; }
    }
}