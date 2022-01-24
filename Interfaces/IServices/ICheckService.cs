namespace Kiwi_review.Interfaces.IServices
{
    public interface ICheckService
    {
        bool CheckMovie(int movieId);
        bool CheckReview(int reviewId);
        bool CheckUser(int userId);
        int? GetUidFromToken(string? token);
        bool CheckRegisterStatus(string? token);
    }
}