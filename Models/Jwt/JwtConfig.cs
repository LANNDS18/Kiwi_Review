namespace Kiwi_review.Models.Jwt
{
    public class JwtConfig
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? IssuerSigningKey { get; set; }
        public int AccessTokenExpiresMinutes { get; set; }
    }
}