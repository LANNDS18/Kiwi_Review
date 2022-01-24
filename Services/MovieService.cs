using System.Diagnostics;
using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Interfaces.IUnitOfWork;
using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.DatabaseModel;
using Kiwi_review.Models.DisplayDto;
using Kiwi_review.Models.UpdateDto;

namespace Kiwi_review.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWorkWrapper _unitOfWork;
        private readonly ICheckService _check;
        private readonly ITopicService _topic;
        private readonly IUserService _user;

        public MovieService(IUnitOfWorkWrapper unitOfWorkWrapper, IUserService userService, 
            ICheckService checkService, ITopicService topicService)
        {
            _unitOfWork = unitOfWorkWrapper;
            _user = userService;
            _check = checkService;
            _topic = topicService;
        }

        public List<MovieShowModel?>? GetAll(string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return null;
            var movies = _unitOfWork.Movies.FindByCondition(
                movie => movie.IsDelete == false && movie.UserId == userId).ToList();
            if (!movies.Any()) return null;
            var movieShowModels = movies.Select(
                movie => TransferMovieShowModel(movie, token, (int) userId)).OrderByDescending(
                movieShowModel =>
                {
                    Debug.Assert(movieShowModel != null, nameof(movieShowModel) + " != null");
                    return movieShowModel.MovieId;
                }).ToList();
            return movieShowModels;
        }

        public MovieShowModel? GetSingle(int movieId, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return null;
            var movie = _unitOfWork.Movies.FindByCondition(
                movie1 => movie1.MovieId == movieId).SingleOrDefault();
            if (movie == null) return null;
            if (!movie.AllowAnonymous && !_check.CheckRegisterStatus(token)) return null;
            var movieShowModel = TransferMovieShowModel(movie, token, (int) userId);
            return movieShowModel;
        }

        public bool Add(MovieAddingModel movie, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId) || movie.MovieDescription == "") return false;
            var user = _user.GetById((int) userId);
            if (user is { IsAnonymousUser: true }) return false;
            var newMovie = new Movie
            {
                MovieDescription = movie.MovieDescription,
                UserId = (int) userId,
                NumberTopic = movie.NumberTopic,
                MaxHighlightPerUser = movie.MaxHighlightPerUser,
                OnlyHighlightOnce = movie.OnlyHighlightOnce,
                AllowAnonymous = movie.AllowAnonymous,
                IsDelete = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _unitOfWork.Movies.Creat(newMovie);
            _unitOfWork.Save();
            var topic = new TopicAddingModel
            {
                MovieId = newMovie.MovieId,
                TopicName = "New Topic"
            };
            for (var i = 0; i < movie.NumberTopic; i++)
            {
                _topic.Add(topic, token);
            }

            return true;
        }

        public bool Delete(int movieId, string? token)
        {
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return false;
            var user = _user.GetById((int) userId);
            if (user is { IsAnonymousUser: true }) return false;
            var movie = _unitOfWork.Movies.FindByCondition(
                movie1 => movie1.MovieId == movieId).SingleOrDefault();
            if (movie == null || movie.UserId != userId) return false;
            if (!movie.AllowAnonymous && !_check.CheckRegisterStatus(token)) return false;
            movie.UpdatedAt = DateTime.Now;
            movie.IsDelete = true;
            var topicList = _topic.GetAll(movieId, token);
            if (topicList != null)
            {
                foreach (var topic in topicList)
                {
                    _topic.Delete(topic.TopicId, token);
                }
            }
            _unitOfWork.Movies.Update(movie);
            _unitOfWork.Save();
            return true;
        }

        public bool Edit(MovieUpdateModel? movieUpdateModel, string? token)
        {
            if (movieUpdateModel == null) return false;
            var userId = _check.GetUidFromToken(token);
            if (userId == null || !_check.CheckUser((int) userId)) return false;
            var user = _user.GetById((int) userId);
            if (user is { IsAnonymousUser: true }) return false;
            var movie = _unitOfWork.Movies.FindByCondition(
                t => t.MovieId == movieUpdateModel.MovieId).SingleOrDefault();
            if (movie == null || movie.UserId != userId) return false;
            movie.MovieDescription = movieUpdateModel.MovieDescription;
            movie.OnlyHighlightOnce = movieUpdateModel.OnlyHighlightOnce;
            movie.UpdatedAt = DateTime.Now;
            if (!movie.AllowAnonymous && !_check.CheckRegisterStatus(token)) return false;
            _unitOfWork.Movies.Update(movie);
            _unitOfWork.Save();
            return true;
        }

        private Movie? UpdateTopicNumber(Movie? movie, string? token)
        {
            if (movie != null)
            {
                var topicShowModels = _topic.GetAll(movie.MovieId, token);
                var movieNumberTopic =  topicShowModels.Count;
                if (movie.NumberTopic == movieNumberTopic) return movie;
                movie.NumberTopic = movieNumberTopic;
            }

            if (movie == null) return movie;
            _unitOfWork.Movies.Update(movie);
            _unitOfWork.Save();
            return movie;
        }

        private MovieShowModel? TransferMovieShowModel(Movie? movie, string? token, int userId)
        {
            if (movie == null) return null;
            movie = UpdateTopicNumber(movie, token);
            Debug.Assert(movie != null, nameof(movie) + " != null");
            var movieShowModel = new MovieShowModel
            {
                CreatedAt = movie.CreatedAt,
                UpdatedAt = movie.UpdatedAt,
                IsDelete = movie.IsDelete,
                MovieId = movie.MovieId,
                OnlyHighlightOnce = movie.OnlyHighlightOnce,
                MaxHighlightPerUser = movie.MaxHighlightPerUser,
                NumberTopic = movie.NumberTopic,
                AllowAnonymous = movie.AllowAnonymous,
                MovieDescription = movie.MovieDescription,
                Editable = movie.UserId == userId,
                MovieTopics = _topic.GetAll(movie.MovieId, token)
            };
            
            return movieShowModel;
        }
    }
}