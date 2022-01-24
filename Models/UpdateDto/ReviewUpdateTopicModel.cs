using System.ComponentModel.DataAnnotations;

namespace Kiwi_review.Models.UpdateDto
{
    public class ReviewUpdateTopicModel
    {
        [Required] public int ReviewId { get; set; }
        [Required] public int NewTopicId { get; set; }
    }
}