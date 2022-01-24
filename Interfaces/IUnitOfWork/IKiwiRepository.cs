using Kiwi_review.Models.DatabaseModel;

namespace Kiwi_review.Interfaces.IUnitOfWork
{
    public interface IHighlightsUnitOfWork : IUnitOfWorkBase<Highlight>
    {
    }

    public interface IReviewsUnitOfWork : IUnitOfWorkBase<Review>
    {
    }

    public interface ITopicsUnitOfWork : IUnitOfWorkBase<Topic>
    {
    }

    public interface IMoviesUnitOfWork : IUnitOfWorkBase<Movie>
    {
    }

    public interface IUsersUnitOfWork : IUnitOfWorkBase<User>
    {
    }
}