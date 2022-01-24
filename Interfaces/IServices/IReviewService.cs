using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.DisplayDto;
using Kiwi_review.Models.UpdateDto;

namespace Kiwi_review.Interfaces.IServices
{
    public interface IReviewService
    {
        List<ReviewShowModel>? GetFromMovie(int movieId, string? token);
        List<ReviewShowModel>? GetFromTopic(int topicId, string? token);
        ReviewShowModel? GetSingle(int reviewId, string? token);
        bool Add(ReviewAddingModel review, string? token);
        bool Delete(int reviewId, string? token);
        bool Edit(ReviewUpdateModel reviewBeforeUpdate, string? token);
        bool UpdateTopic(int topicId, int reviewId, string? token);
    }
}