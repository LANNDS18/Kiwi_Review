using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.UpdateDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiwi_review.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _review;

        public ReviewController(IReviewService reviewService)
        {
            _review = reviewService;
        }

        [HttpGet("movie/{movieId:int}")]
        public IActionResult GetAll(int movieId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            var ideas = _review.GetFromMovie(movieId, token);
            if (ideas != null) return Ok(ideas);
            return NotFound();
        } 

        [HttpGet("topic/{topicId:int}")]
        public IActionResult GetFromTopic(int topicId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            var ideas = _review.GetFromTopic(topicId, token);
            if (ideas != null) return Ok(ideas);
            return NotFound();
        }


        [HttpGet("{reviewId:int}")]
        public IActionResult GetSingle(int reviewId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            var reviewShowModel = _review.GetSingle(reviewId, token);
            if (reviewShowModel == null) return NotFound();
            return Ok(reviewShowModel);
        }


        [HttpPost]
        public IActionResult Add(ReviewAddingModel review)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_review.Add(review, token)) return Ok();
            return BadRequest();
        }


        [HttpDelete("{reviewId:int}")]
        public IActionResult Delete(int reviewId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_review.Delete(reviewId, token)) return Ok();
            return BadRequest();
        }


        [HttpPut]
        public IActionResult Edit(ReviewUpdateModel review)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_review.Edit(review, token)) return Ok();
            return BadRequest();
        }


        [HttpPut("topic")]
        public IActionResult UpdateTopic(ReviewUpdateTopicModel review)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_review.UpdateTopic(topicId:review.NewTopicId, reviewId:review.ReviewId, token)) return Ok();
            return BadRequest();
        }
    }
}