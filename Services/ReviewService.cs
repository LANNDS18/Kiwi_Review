using System.Diagnostics;
using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Interfaces.IUnitOfWork;
using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.DatabaseModel;
using Kiwi_review.Models.DisplayDto;
using Kiwi_review.Models.UpdateDto;

namespace Kiwi_review.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWorkWrapper _unitOfWork;
        private readonly ICheckService _check;
        private readonly IHighlightService _highlight;

        public ReviewService(IUnitOfWorkWrapper unitOfWorkWrapper, ICheckService checkService, IHighlightService highlightService)
        {
            _unitOfWork = unitOfWorkWrapper;
            _check = checkService;
            _highlight = highlightService;
        }

        public List<ReviewShowModel>? GetFromMovie(int movieId, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckMovie(movieId) || !_check.CheckUser((int) userId)) return null;
            var reviews = _unitOfWork.Reviews.FindByCondition(review =>
                review.MovieId == movieId && review.IsDelete == false).ToList();
            var movie = _unitOfWork.Movies.FindByCondition(movie1 => movie1.MovieId == movieId).SingleOrDefault();
            if (movie is {AllowAnonymous: false} && !_check.CheckRegisterStatus(token)) return null;
            if (!reviews.Any()) return null;
            var reviewShowModels = new List<ReviewShowModel>();
            reviewShowModels.AddRange(reviews.Select(review => TransferReviewShowModel(review, token, (int) userId))
                .OrderByDescending(reviewShowModel =>
                {
                    Debug.Assert(reviewShowModel != null, nameof(reviewShowModel) + " != null");
                    return reviewShowModel.NumberHighlight;
                })!);
            return reviewShowModels;
        }

        public List<ReviewShowModel>? GetFromTopic(int topicId, string? token)
        {
            var topic = _unitOfWork.Topics.FindByCondition(c => c.TopicId == topicId).SingleOrDefault();
            if (topic == null) return null!;
            var movie = _unitOfWork.Movies.FindByCondition(t => t.MovieId == topic.MovieId).SingleOrDefault();
            if (movie is {AllowAnonymous: false} && !_check.CheckRegisterStatus(token)) return null!;
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return null!;
            var reviews = _unitOfWork.Reviews.FindByCondition(
                review => review.TopicId == topicId && review.IsDelete == false).ToList();
            if (!reviews.Any()) return null!;
            var reviewShowModels = new List<ReviewShowModel>();
            reviewShowModels.AddRange(reviews.Select(review => 
                TransferReviewShowModel(review, token, (int)userId))!);
            return !reviewShowModels.Any() ? null : reviewShowModels;
        }

        public ReviewShowModel? GetSingle(int reviewId, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return null;
            if (reviewId < 0) return null;
            var review = _unitOfWork.Reviews.FindByCondition(
                review1 => review1.ReviewId == reviewId && review1.IsDelete == false).SingleOrDefault();
            if (review == null) return null;
            var movie = _unitOfWork.Movies.FindByCondition(t => t.MovieId == review.MovieId).SingleOrDefault();
            if (movie is {AllowAnonymous: false} && !_check.CheckRegisterStatus(token)) return null;
            var reviewShowModel = TransferReviewShowModel(review, token, (int) userId);
            return reviewShowModel;
        }

        public bool Add(ReviewAddingModel review, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return false;
            var addReview = new Review
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDelete = false,
                UserId = (int) userId,
                ReviewContent = review.ReviewContent,
                MovieId = review.MovieId,
                TopicId = review.TopicId
            };
            _unitOfWork.Reviews.Creat(addReview);
            _unitOfWork.Save();
            return true;
        }

        public bool Delete(int reviewId, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return false;
            var review = _unitOfWork.Reviews.FindByCondition(
                r => r.ReviewId == reviewId).SingleOrDefault();
            if (review == null) return false;
            var movie = _unitOfWork.Movies.FindByCondition(movie1 => movie1.MovieId == review.MovieId).SingleOrDefault();
            if (movie != null && movie.UserId != userId && review.UserId != userId) return false;
            var highlightList = _highlight.GetAll(reviewId, token);
            foreach (var t in highlightList)
            {
                _highlight.Delete(t.HighlightId);
            }
            review.IsDelete = true;
            review.UpdatedAt = DateTime.Now;
            _unitOfWork.Reviews.Update(review);
            _unitOfWork.Save();
            return true;
        }

        public bool Edit(ReviewUpdateModel reviewBeforeUpdate, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return false;
            var review = _unitOfWork.Reviews.FindByCondition(
                review1 => review1.ReviewId == reviewBeforeUpdate.ReviewId).SingleOrDefault();
            if (review == null) return false;
            var movie = _unitOfWork.Movies.FindByCondition(movie1 => movie1.MovieId == review.MovieId).SingleOrDefault();
            if (movie != null && movie.UserId != userId && review.UserId != userId) return false;
            review.UpdatedAt = DateTime.Now;
            review.ReviewContent = reviewBeforeUpdate.ReviewContent;
            _unitOfWork.Reviews.Update(review);
            _unitOfWork.Save();
            return true;
        }

        public bool UpdateTopic(int topicId, int reviewId, string? token)
        {
            if (!_check.CheckReview(reviewId)) return false;
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return false;
            var review = _unitOfWork.Reviews.FindByCondition(
                review1 => review1.ReviewId == reviewId).SingleOrDefault();
            if (review == null) return false;
            var topic = _unitOfWork.Topics.FindByCondition(
                topic1 => topic1.TopicId == topicId).SingleOrDefault();
            if (topic == null || topic.MovieId != review.MovieId) return false;
            review.UpdatedAt = DateTime.Now;
            review.TopicId = topicId;
            _unitOfWork.Reviews.Update(review);
            _unitOfWork.Save();
            return true;
        }

        private ReviewShowModel? TransferReviewShowModel(Review? review, string? token, int userId)
        {
            if (review == null) return null;
            var reviewDisplay = new ReviewShowModel
            {
                ReviewId = review.ReviewId,
                ReviewContent = review.ReviewContent,
                TopicId = review.TopicId,
                MovieId = review.MovieId,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                IsDelete = review.IsDelete,
                ReviewHighlights = _highlight.GetAll(review.ReviewId, token),
                Editable = review.UserId == userId,
            };
            reviewDisplay.NumberHighlight = reviewDisplay.ReviewHighlights == null ? 0 : reviewDisplay.ReviewHighlights.Count;
            var movie = _unitOfWork.Movies.FindByCondition(t => t.MovieId == review.MovieId).SingleOrDefault();
            if (movie != null && movie.UserId == userId) reviewDisplay.Editable = true;
            var totalHighlight = _highlight.MovieHighlightCountPerUser(review.MovieId, userId);
            if (movie != null && (!totalHighlight.Any() || totalHighlight.Count < movie.MaxHighlightPerUser))
            {
                reviewDisplay.Highlightable = true;
            }
            else
            {
                reviewDisplay.Highlightable = false;
            }

            if (movie is {OnlyHighlightOnce: true} && !_highlight.CheckOnlyOneHighlightPerReview(review.ReviewId, userId))
                reviewDisplay.Highlightable = false;

            var highlights = _unitOfWork.Highlights.FindByCondition(l => l.ReviewId == review.ReviewId).ToList();
            if (!highlights.Any()) return reviewDisplay;
            var alreadyHighlighted = highlights.Where(
                    highlight => highlight.UserId == userId && highlight.IsDelete == false).Select(highlight => _highlight.ShowHighlight(highlight))
                .ToList();
            reviewDisplay.AlreadyHighlighted = alreadyHighlighted;
            return reviewDisplay;
        }
    }
}