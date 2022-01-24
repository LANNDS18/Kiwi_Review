using System.Collections.Generic;
using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.DatabaseModel;
using Kiwi_review.Models.DisplayDto;

namespace Kiwi_review.Interfaces.IServices
{
    public interface IHighlightService
    {
        List<HighlightShowModel> GetAll(int reviewId, string token);
        bool Add(HighlightAddingModel highlight, string? token);
        bool Cancel(int highlightId, string? token);
        void Delete(int highlightId);
        List<Highlight> MovieHighlightCountPerUser(int movieId, int userId);
        HighlightShowModel ShowHighlight(Highlight highlight);
        bool CheckOnlyOneHighlightPerReview(int reviewId, int userId);
    }
}