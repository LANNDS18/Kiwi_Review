using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Models.AddingDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiwi_review.Controllers
{
    [ApiController]
    [Route("api/highlight")]
    [Authorize]
    public class HighlightsController : Controller
    {
        private readonly IHighlightService _highlight;

        public HighlightsController (IHighlightService highlightService)
        {
            _highlight = highlightService;
        }


        [HttpGet("{reviewId:int}")]
        public IActionResult GetAll(int reviewId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            var likes = _highlight.GetAll(reviewId, token);
            return Ok(likes);
        }


        [HttpPost]
        public IActionResult Add(HighlightAddingModel like)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_highlight.Add(like, token)) return Ok();
            return BadRequest();
        }


        [HttpDelete("{highlightId:int}")]
        public IActionResult Cancel(int highlightId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_highlight.Cancel(highlightId, token)) return Ok();
            return BadRequest();
        }
    }
}