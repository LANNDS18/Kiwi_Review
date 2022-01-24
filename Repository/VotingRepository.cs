using Kiwi_review.Interfaces.IUnitOfWork;
using Kiwi_review.Models;
using Kiwi_review.Models.DatabaseModel;

namespace Kiwi_review.Repository
{
    public class HighlightsUnitOfWork : UnitOfWorkBase<Highlight>, IHighlightsUnitOfWork
    {
        public HighlightsUnitOfWork(KiwiReviewContext kiwiReviewContext) : base(kiwiReviewContext)
        {
        }
    }

    public class ReviewsUnitOfWork : UnitOfWorkBase<Review>, IReviewsUnitOfWork
    {
        public ReviewsUnitOfWork(KiwiReviewContext kiwiReviewContext) : base(kiwiReviewContext)
        {
        }
    }

    public class TopicsUnitOfWork : UnitOfWorkBase<Topic>, ITopicsUnitOfWork
    {
        public TopicsUnitOfWork(KiwiReviewContext kiwiReviewContext) : base(kiwiReviewContext)
        {
        }
    }

    public class MoviesUnitOfWork : UnitOfWorkBase<Movie>, IMoviesUnitOfWork
    {
        public MoviesUnitOfWork(KiwiReviewContext kiwiReviewContext) : base(kiwiReviewContext)
        {
        }
    }

    public class UsersUnitOfWork : UnitOfWorkBase<User>, IUsersUnitOfWork
    {
        public UsersUnitOfWork(KiwiReviewContext kiwiReviewContext) : base(kiwiReviewContext)
        {
        }
    }
}