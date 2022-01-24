using System.ComponentModel.DataAnnotations;
using Kiwi_review.Models.DatabaseModel;

namespace Kiwi_review.Models.DisplayDto
{
    public class ReviewShowModel : BaseEntity
    {
        [Key] public int ReviewId { get; set; }
        public string ReviewContent { get; set; }
        public int MovieId { get; set; }
        public int TopicId { get; set; }
        public bool Editable { get; set; }
        public bool Highlightable { get; set; }
        public List<HighlightShowModel> AlreadyHighlighted { get; set; }
        public int NumberHighlight { get; set; } 
        public List<HighlightShowModel> ReviewHighlights { get; set; }
    }
}