using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Interfaces.IUnitOfWork;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Kiwi_review.Services
{
    public class CheckService : ICheckService
    {
        private readonly IUnitOfWorkWrapper _unitOfWork;

        public CheckService(IUnitOfWorkWrapper unitOfWorkWrapper)
        {
            _unitOfWork = unitOfWorkWrapper;
        }

        public bool CheckMovie(int movieId)
        {
            if (movieId < 0) return false;
            var thisMovie = _unitOfWork.Movies.FindByCondition(
                t => t.MovieId == movieId).SingleOrDefault();
            return thisMovie is {IsDelete: false};
        }


        public bool CheckReview(int reviewId)
        {
            if (reviewId < 0) return false;
            var thisReview = _unitOfWork.Reviews.FindByCondition(
                i => i.ReviewId == reviewId).SingleOrDefault();
            return thisReview is {IsDelete: false};
        }


        public bool CheckUser(int userId)
        {
            var thisUser = _unitOfWork.Users.FindByCondition(
                u => u.UserId == userId).SingleOrDefault();
            return thisUser is {IsDelete: false};
        }

        public int? GetUidFromToken(string? token)
        {
            var jwtArr = token.Split('.');
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[1]));
            var uidString = payLoad["userId"];
            if (uidString == null) return null;
            return int.Parse(uidString);
        }
        
        public bool CheckRegisterStatus(string? token)
        {
            var userId = GetUidFromToken(token);
            if (userId == null) return false;
            var user = _unitOfWork.Users.FindByCondition(
                u => u.UserId == userId).SingleOrDefault();
            if (user == null || user.IsAnonymousUser || user.IsDelete) return false;
            return !user.IsAnonymousUser;
        }
    }
}