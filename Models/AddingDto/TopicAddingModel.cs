using System.ComponentModel.DataAnnotations;

namespace Kiwi_review.Models.AddingDto
{
    public class TopicAddingModel
    {
        [Required] public string? TopicName { get; set; }
        [Required] public int MovieId { get; set; }
    }
}