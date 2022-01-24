using System.ComponentModel.DataAnnotations;

namespace Kiwi_review.Models.UpdateDto
{
    public class ReviewUpdateModel
    {
        [Required] public int ReviewId { get; set; }
        [MinLength(1)]
        [MaxLength(2000)]
        public string ReviewContent { get; set; }
    }
}