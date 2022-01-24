using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.DisplayDto;
using Kiwi_review.Models.UpdateDto;

namespace Kiwi_review.Interfaces.IServices
{
    public interface IMovieService
    {
        List<MovieShowModel?>? GetAll(string? token);
        MovieShowModel? GetSingle(int movieId, string? token);
        bool Add(MovieAddingModel movie, string? token);
        bool Delete(int movieId, string? token);
        bool Edit(MovieUpdateModel? movieUpdateModel, string? token);
    }
}