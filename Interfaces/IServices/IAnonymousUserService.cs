using Kiwi_review.Models.DisplayDto;

namespace Kiwi_review.Interfaces.IServices
{
    public interface IAnonymousUserService
    {
        AnonymousUserShowModel Generate();
        AnonymousUserShowModel Get(int userId);
        string? RefreshToken(string? token);
        TokenType CheckTokenStatus(string? token);
        AnonymousUserShowModel AutoGenerate(string? token);
    }
}