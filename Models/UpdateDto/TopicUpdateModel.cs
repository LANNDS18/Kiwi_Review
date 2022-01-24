using System.ComponentModel.DataAnnotations;

namespace Kiwi_review.Models.UpdateDto
{
    public class TopicUpdateModel
    {
        [Required] public int TopicId { get; set; }
        [Required] public string? TopicName { get; set; }
        public int MovieId { get; set; }
    }
}