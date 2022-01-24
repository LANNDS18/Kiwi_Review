namespace Kiwi_review.Models.DatabaseModel
{
    public class Highlight : BaseEntity
    {
        public int HighlightId { get; set; }
        public int ReviewId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }

    }
}