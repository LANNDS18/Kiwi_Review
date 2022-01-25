using Kiwi_review.Interfaces.IUnitOfWork;
using Kiwi_review.Models;

namespace Kiwi_review.UnitOfWork
{
    public class UnitOfWorkWrapper : IUnitOfWorkWrapper
    {
        private readonly KiwiReviewContext _context;
        private IHighlightsUnitOfWork _highlights;
        private IReviewsUnitOfWork _reviews;
        private ITopicsUnitOfWork _topics;
        private IMoviesUnitOfWork _movies;
        private IUsersUnitOfWork _users;

        public IHighlightsUnitOfWork Highlights => _highlights ??= new HighlightsUnitOfWork(_context);
        public IReviewsUnitOfWork Reviews => _reviews ??= new ReviewsUnitOfWork(_context);
        public ITopicsUnitOfWork Topics => _topics ??= new TopicsUnitOfWork(_context);
        public IMoviesUnitOfWork Movies => _movies ??= new MoviesUnitOfWork(_context);
        public IUsersUnitOfWork Users => _users ??= new UsersUnitOfWork(_context);

        public UnitOfWorkWrapper(KiwiReviewContext kiwiReviewContext)
        {
            _context = kiwiReviewContext;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}