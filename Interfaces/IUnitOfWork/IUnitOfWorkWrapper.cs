namespace Kiwi_review.Interfaces.IUnitOfWork
{
    public interface IUnitOfWorkWrapper
    {
        IMoviesUnitOfWork Movies { get; }
        ITopicsUnitOfWork Topics { get; }
        IReviewsUnitOfWork Reviews { get; }
        IHighlightsUnitOfWork Highlights { get; }
        IUsersUnitOfWork Users { get; }
        void Save();
    }
}