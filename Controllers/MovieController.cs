using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.UpdateDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiwi_review.Controllers
{
    [ApiController]
    [Route("api/movies")]
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieService _movie;

        public MovieController(IMovieService movieService)
        {
            _movie = movieService;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            var movies = _movie.GetAll(token);
            if (movies == null) return NotFound();
            return Ok(movies);
        }


        [HttpGet("{movieId:int}")]
        public IActionResult GetSingle(int movieId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            var movies = _movie.GetSingle(movieId, token);
            if (movies == null) return NotFound();
            return Ok(movies);
        }

        [HttpPost]
        public IActionResult Add(MovieAddingModel movieAddingModel)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_movie.Add(movieAddingModel, token)) return Ok();
            return BadRequest();
        }

        [HttpPut]
        public IActionResult Edit(MovieUpdateModel? movieUpdateModel)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_movie.Edit(movieUpdateModel, token)) return Ok();
            return BadRequest();
        }

        [HttpDelete("{movieId:int}")]
        public IActionResult Delete(int movieId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_movie.Delete(movieId, token)) return Ok();
            return BadRequest();
        }
    }
}