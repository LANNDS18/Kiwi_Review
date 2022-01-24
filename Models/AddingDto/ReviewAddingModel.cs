using System.ComponentModel.DataAnnotations;

namespace Kiwi_review.Models.AddingDto
{
    public class ReviewAddingModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string ReviewContent { get; set; }

        [Required] public int MovieId { get; set; }
        
        public int TopicId { get; set; }
    }
}