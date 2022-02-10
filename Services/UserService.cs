using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Interfaces.IUnitOfWork;
using Kiwi_review.Models;
using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.DatabaseModel;
using Kiwi_review.Models.DisplayDto;
using Kiwi_review.Models.Jwt;
using Microsoft.AspNetCore.Identity;

namespace Kiwi_review.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWorkWrapper _unitOfWork;
        private readonly ITokenService _token;
        private readonly ICheckService _check;

        public UserService(IUnitOfWorkWrapper unitOfWorkWrapper, ITokenService token, ICheckService check)
        {
            _unitOfWork = unitOfWorkWrapper;
            _token = token;
            _check = check;
        }

        public UserShowModel? Register(UserAddingModel userRegister)
        {
            var checkUser = _unitOfWork.Users.FindByCondition(
                u => u.Email == userRegister.Email && u.IsDelete == false).SingleOrDefault();
            if (checkUser != null) return null;
            var passwordHasher = new PasswordHasher<UserAddingModel>();
            var hashPassword = passwordHasher.HashPassword(userRegister, userRegister.Password);
            var newUser = new User
            {
                IsAnonymousUser = false,
                Alias = userRegister.Description,
                Description = userRegister.Alias,
                Email = userRegister.Email,
                Password = hashPassword,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsDelete = false
            };
            _unitOfWork.Users.Creat(newUser);
            _unitOfWork.Save();
            var displayUser = _unitOfWork.Users.FindByCondition(
                u => u.Email == userRegister.Email).SingleOrDefault();
            if (displayUser == null) return null;
            var displayDto = GetById(displayUser.UserId);
            var token = TokenCreate(displayDto);
            if (displayDto == null) return null;
            displayDto.JwtToken = token;
            return displayDto;
        }

        public UserShowModel? Login(UserLoginDto user)
        {
            if (user.Email == null || user.Password == null) return null;
            var existing = _unitOfWork.Users.FindByCondition(
                u => u.Email == user.Email
                     && u.IsAnonymousUser == false && u.IsDelete == false).SingleOrDefault();
            var hashish = new PasswordHasher<User>();
            if (existing == null ||
                0 == hashish.VerifyHashedPassword(existing,
                    existing.Password, user.Password)) return null;
            var displayDto = GetById(existing.UserId);
            if (displayDto == null) return null;
            var token = TokenCreate(displayDto);
            displayDto.JwtToken = token;
            return displayDto;
        }

        public UserShowModel? Get(string? token)
        {
            var userid = _check.GetUidFromToken(token);
            var thisUser = _unitOfWork.Users.FindByCondition(
                u => u.UserId == userid).SingleOrDefault();
            if (thisUser == null) return null;
            var displayDto = new UserShowModel
            {
                UserId = thisUser.UserId,
                Alias = thisUser.Alias,
                Email = thisUser.Email,
                Description = thisUser.Description,
                CreatedAt = thisUser.CreatedAt,
                UpdatedAt = thisUser.UpdatedAt,
                IsDelete = false,
                IsAnonymousUser = thisUser.IsAnonymousUser
            };
            return displayDto;
        }

        public UserShowModel? GetById(int userid)
        {
            var thisUser = _unitOfWork.Users.FindByCondition(
                u => u.UserId == userid &&
                     u.IsDelete == false).SingleOrDefault();
            if (thisUser == null) return null;
            var displayDto = new UserShowModel
            {
                UserId = thisUser.UserId,
                Alias = thisUser.Alias,
                Email = thisUser.Email,
                Description = thisUser.Description,
                CreatedAt = thisUser.CreatedAt,
                UpdatedAt = thisUser.UpdatedAt,
                IsDelete = false,
                IsAnonymousUser = thisUser.IsAnonymousUser
            };
            return displayDto;
        }

        private TnToken? TokenCreate(UserShowModel? userShowModel)
        {
            var keyValuePairs = new Dictionary<string, string?>
            {
                {"userId", userShowModel?.UserId.ToString()},
                {"isAnonymousUser", userShowModel?.IsAnonymousUser.ToString()},
                {"Email", userShowModel?.Email}
            };
            var token = _token.CreateToken(keyValuePairs);
            return token;
        }
    }
}