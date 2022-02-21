using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Models.DisplayDto;
using Microsoft.AspNetCore.Mvc;

namespace Kiwi_review.Controllers
{
    [ApiController]
    [Route("api/anonymous")]
    public class AnonymousUsersController : Controller
    {
        private readonly IAnonymousUserService _anonymous;
        private readonly IJwtService _jwt;
        public AnonymousUsersController(IAnonymousUserService anonymousUserService, IJwtService jwtService)
        {
            _anonymous = anonymousUserService;
            _jwt = jwtService;

        }

        [HttpPost]
        public IActionResult Add()
        {
            var thisUserDto = _anonymous.Generate();
            return Ok(thisUserDto);
        }

        [HttpPost("autoGenerate")]
        public IActionResult AutoGenerate(string? token)
        {
            if (token == null) return Ok(_anonymous.Generate());
            var user = _anonymous.AutoGenerate(token);
            if (user == null) return BadRequest();
            return Ok(user);
        }


        [HttpPost("tokenRefresh")]
        public IActionResult RefreshToken(string? token)
        {
            if (token == null) return NotFound();
            var resetToken = _anonymous.RefreshToken(token);
            if (resetToken == null) return BadRequest();
            return Ok(_jwt.GetEntireToken(token));
        }

        [HttpGet("checkTokenStatus")]
        public IActionResult CheckTokenStatus(string? token)
        {
            var resetToken = _anonymous.CheckTokenStatus(token).ToString();
            return Ok(resetToken);
        }
    }
}