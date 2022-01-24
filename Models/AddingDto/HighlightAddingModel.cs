using System.ComponentModel.DataAnnotations;

namespace Kiwi_review.Models.AddingDto
{
    public class HighlightAddingModel
    {
        [Required] public int ReviewId { get; set; }
        [Required] public int MovieId { get; set; }
    }
}