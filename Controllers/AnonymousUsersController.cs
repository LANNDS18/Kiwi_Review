using Kiwi_review.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Kiwi_review.Controllers
{
    [ApiController]
    [Route("api/anonymous")]
    public class AnonymousUsersController : Controller
    {
        private readonly IAnonymousUserService _anonymous;
        private readonly ITokenService _token;
        public AnonymousUsersController(IAnonymousUserService anonymousUserService, ITokenService tokenService)
        {
            _anonymous = anonymousUserService;
            _token = tokenService;

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
            return Ok(_token.GetEntireToken(token));
        }

        [HttpGet("checkTokenStatus")]
        public IActionResult CheckTokenStatus(string? token)
        {
            var resetToken = _anonymous.CheckTokenStatus(token).ToString();
            return Ok(resetToken);
        }
    }
}