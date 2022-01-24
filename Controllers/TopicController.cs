using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Models.AddingDto;
using Kiwi_review.Models.UpdateDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiwi_review.Controllers
{
    [ApiController]
    [Route("api/topic")]
    [Authorize]
    public class TopicController : Controller
    {
        private readonly ITopicService _topic;

        public TopicController(ITopicService topicService)
        {
            _topic = topicService;
        }

        [HttpGet("{movieId:int}")]
        public IActionResult GetAll(int movieId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            var columns = _topic.GetAll(movieId, token);
            if (columns == null) return NotFound();
            return Ok(columns);
        }

        [HttpPost]
        public IActionResult Add(TopicAddingModel topic)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_topic.Add(topic, token)) return Ok();
            return BadRequest();
        }

        [HttpDelete("{topicId:int}")]
        public IActionResult Delete(int topicId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_topic.Delete(topicId, token)) return Ok();
            return BadRequest();
        }

        [HttpPut]
        public IActionResult Update(TopicUpdateModel column)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token == null) return Unauthorized();
            if (_topic.Update(column, token)) return Ok();
            return BadRequest();
        }
    }
}