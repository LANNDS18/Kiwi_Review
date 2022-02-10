using Kiwi_review.Models;
using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.DisplayDto;

namespace Kiwi_review.Interfaces.IServices
{
    public interface IUserService
    {
        UserShowModel? Register(UserAddingModel userRegister);
        UserShowModel? Login(UserLoginDto user);
        UserShowModel? Get(string? token);
        UserShowModel? GetById(int userid);
    }
}