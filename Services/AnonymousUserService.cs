using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Interfaces.IUnitOfWork;
using Kiwi_review.Models.DatabaseModel;
using Kiwi_review.Models.DisplayDto;
using Kiwi_review.Models.Jwt;

namespace Kiwi_review.Services
{
    public class AnonymousUserService : IAnonymousUserService
    {
        private readonly IUnitOfWorkWrapper _unitOfWork;
        private readonly IJwtService _jwt;
        private readonly ICheckService _check;

        public AnonymousUserService(IUnitOfWorkWrapper unitOfWorkWrapper, IJwtService jwt,
            ICheckService checkService)
        {
            _unitOfWork = unitOfWorkWrapper;
            _jwt = jwt;
            _check = checkService;
        }

        public AnonymousUserShowModel? Generate()
        {
            var newUser = new User
            {
                Email = "AnonymousUser",
                Alias = "AnonymousUser",
                Description = "AnonymousUser",
                Password = null,
                IsAnonymousUser = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDelete = false
            };
            _unitOfWork.Users.Creat(newUser);
            _unitOfWork.Save();
            newUser.Email = $"{newUser.UserId}@anonymousUser";
            newUser.Alias = $"anonymousUser:{newUser.UserId}";
            _unitOfWork.Users.Update(newUser);
            _unitOfWork.Save();
            var user = Get(newUser.UserId);
            if (user == null) return null;
            user.JwtToken = TokenCreate(user);
            return user;
        }

        public AnonymousUserShowModel? Get(int userId)
        {
            var thisUser = _unitOfWork.Users.FindByCondition(
                u => u.UserId == userId &&
                     u.IsDelete == false && u.IsAnonymousUser).SingleOrDefault();
            if (thisUser == null) return null;
            var unregisteredUserDto = new AnonymousUserShowModel
            {
                CreatedAt = thisUser.CreatedAt,
                UpdatedAt = thisUser.UpdatedAt,
                IsDelete = thisUser.IsDelete,
                IsAnonymousUser = thisUser.IsAnonymousUser,
                UserId = thisUser.UserId,
            };
            return unregisteredUserDto;
        }

        public string? RefreshToken(string? token)
        {
            try
            {
                var tokenType = _jwt.ValidateTokenState(token);
                switch (tokenType)
                {
                    case TokenType.Ok:
                        return token;
                    case TokenType.Fail:
                        return null;
                    case TokenType.Expired:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var userId = _check.GetUidFromToken(token);
                if (userId == null || !_check.CheckUser((int) userId)) return null;
                var user = Get((int) userId);
                var reset = TokenCreate(user);
                return reset?.TokenStr;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public AnonymousUserShowModel? AutoGenerate(string? token)
        {
            var newToken = RefreshToken(token);
            if(newToken==null) return Generate();
            var userid = _check.GetUidFromToken(token);
            if (userid == null) return Generate();
            var user = Get((int)userid);
            if (user == null) return null;
            user.JwtToken = _jwt.GetEntireToken(newToken);
            return user;
        }

        public TokenType CheckTokenStatus(string? token)
        {
            try
            {
                var tokenType = _jwt.ValidateTokenState(token);
                return tokenType;
            }
            catch (Exception)
            {
                return TokenType.Fail;
            }
        }

        private TnToken? TokenCreate(AnonymousUserShowModel? userDisplayDto)
        {
            var keyValuePairs = new Dictionary<string, string?>
            {
                {"userId", userDisplayDto?.UserId.ToString()},
                {"isAnonymousUser", userDisplayDto?.IsAnonymousUser.ToString()},
                {"Email", $"{userDisplayDto?.UserId}@tempUser"},
            };
            var token = _jwt.CreateToken(keyValuePairs);
            return token;
        }
    }
}