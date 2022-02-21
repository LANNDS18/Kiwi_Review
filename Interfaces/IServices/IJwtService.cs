using Kiwi_review.Models.Jwt;

namespace Kiwi_review.Interfaces.IServices
{
    public interface IJwtService
    {
        TnToken? CreateToken(Dictionary<string, string?> keyValuePairs);
        TokenType ValidateTokenState(string? encodeJwt);
        TnToken? GetEntireToken(string? tkStr);
    }

    public enum TokenType
    {
        Ok,
        Fail,
        Expired
    }
}