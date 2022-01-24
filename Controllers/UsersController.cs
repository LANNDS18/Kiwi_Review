using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Models;
using Kiwi_review.Models.AddingDto;
using Microsoft.AspNetCore.Mvc;

namespace Kiwi_review.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService _user;

        public UsersController(IUserService userService)
        {
            _user = userService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserAddingModel userRegister)
        {
            var newUser = _user.Register(userRegister);
            if (newUser == null) return NoContent();
            return Ok(newUser);
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserLoginDto user)
        {
            var thisUser = _user.Login(user);
            if (thisUser == null) return Unauthorized();
            return Ok(thisUser);
        }
        

        [HttpGet]
        [Route("{token}")]
        public IActionResult Get(string? token)
        {
            var displayDto = _user.Get(token);
            if (displayDto == null) return NotFound();
            return Ok(displayDto);
        }
    }
}