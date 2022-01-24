using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Interfaces.IUnitOfWork;
using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.DatabaseModel;
using Kiwi_review.Models.DisplayDto;
using Kiwi_review.Models.UpdateDto;

namespace Kiwi_review.Services
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWorkWrapper _unitOfWork;
        private readonly ICheckService _check;
        private readonly IReviewService _review;

        public TopicService(IUnitOfWorkWrapper unitOfWorkWrapper, ICheckService checkService, IReviewService reviewService)
        {
            _unitOfWork = unitOfWorkWrapper;
            _check = checkService;
            _review = reviewService;
        }

        public List<TopicShowModel?>? GetAll(int movieId, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId) || !_check.CheckMovie(movieId)) return null;
            var topics = _unitOfWork.Topics.FindByCondition(
                topic => topic.MovieId == movieId && topic.IsDelete == false).ToList();
            if (!topics.Any()) return null;
            var showModels = topics.Select(
                topic => TransferTopicDisplayDto(topic, token, (int) userId)).ToList();
            return !showModels.Any() ? null : showModels;
        }

        public bool Add(TopicAddingModel topic, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)
                               || !_check.CheckMovie(topic.MovieId)) return false;
            var movie = _unitOfWork.Movies.FindByCondition(
                movie1 => movie1.MovieId == topic.MovieId).SingleOrDefault();
            if (movie == null || movie.UserId != userId) return false;
            var newTopic = new Topic
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDelete = false,
                MovieId = topic.MovieId,
                TopicName = topic.TopicName
            };
            _unitOfWork.Topics.Creat(newTopic);
            _unitOfWork.Save();
            return true;
        }
        
        
        public bool Delete(int topicId, string token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return false;
            var topic = _unitOfWork.Topics.FindByCondition(
                topic1 => topic1.TopicId == topicId).SingleOrDefault();
            if (topic == null
                || !_check.CheckMovie(topic.MovieId)) return false;
            var movie = _unitOfWork.Movies.FindByCondition(
                movie1 => movie1.MovieId == topic.MovieId).SingleOrDefault();
            if (movie == null || movie.UserId != userId) return false;
            if (movie.NumberTopic == 0) return false;
            topic.IsDelete = true;
            topic.UpdatedAt = DateTime.Now;
            var reviews = _review.GetFromTopic(topic.TopicId, token);
            if (reviews != null)
            {
                foreach (var review in reviews)
                {
                    _review.Delete(review.ReviewId, token);
                }
            }
            _unitOfWork.Topics.Update(topic);
            _unitOfWork.Save();
            return true;
        }

        public bool Update(TopicUpdateModel topic, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)
                               || !_check.CheckMovie(topic.MovieId)) return false;
            var thisTopic = _unitOfWork.Topics.FindByCondition(
                topic1 => topic1.TopicId == topic.TopicId).SingleOrDefault();
            if (thisTopic == null) return false;
            var thisMovie = _unitOfWork.Movies.FindByCondition(
                movie => movie.MovieId == thisTopic.MovieId).SingleOrDefault();
            if (thisMovie == null || thisMovie.UserId != userId) return false;
            thisTopic.TopicName = topic.TopicName;
            thisTopic.UpdatedAt = DateTime.Now;
            _unitOfWork.Topics.Update(thisTopic);
            _unitOfWork.Save();
            return true;
        }

        private TopicShowModel? TransferTopicDisplayDto(Topic? topic, string? token, int userId)
        {
            if (topic == null) return null;
            var topicShowModel = new TopicShowModel
            {
                CreatedAt = topic.CreatedAt,
                UpdatedAt = topic.UpdatedAt,
                IsDelete = topic.IsDelete,
                TopicId = topic.TopicId,
                TopicName = topic.TopicName, 
                MovieId = topic.MovieId,
                TopicReviews = _review.GetFromTopic(topic.TopicId, token)
            };
            var movie = _unitOfWork.Movies.FindByCondition(movie1 => movie1.MovieId == topic.MovieId).SingleOrDefault();
            if (movie != null) topicShowModel.Editable = movie.UserId == userId;
            if (movie is {AllowAnonymous: false} && !_check.CheckRegisterStatus(token)) return null;
            return topicShowModel;
        }
    }
}