using System.ComponentModel.DataAnnotations;

namespace Kiwi_review.Models.UpdateDto
{
    public class MovieUpdateModel
    {
        [Required] public int MovieId { get; set; }
        [MaxLength(200)] public string MovieDescription { get; set; }
        public bool OnlyHighlightOnce { get; set; }
    }
}