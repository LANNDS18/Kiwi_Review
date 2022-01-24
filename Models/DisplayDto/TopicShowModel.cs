using Kiwi_review.Models.DatabaseModel;

namespace Kiwi_review.Models.DisplayDto
{
    public class TopicShowModel : BaseEntity
    {
        public int TopicId { get; set; }
        public string? TopicName { get; set; }
        public int MovieId { get; set; }
        public bool Editable { get; set; }
        public List<ReviewShowModel>? TopicReviews { get; set; }
    }
}