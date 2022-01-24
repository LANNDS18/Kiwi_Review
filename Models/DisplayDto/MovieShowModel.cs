using Kiwi_review.Models.DatabaseModel;

namespace Kiwi_review.Models.DisplayDto
{
    public class MovieShowModel : BaseEntity
    {
        public int MovieId { get; set; }
        public string MovieDescription { get; set; }
        public bool AllowAnonymous { get; set; }
        public int NumberTopic { get; set; }
        public int MaxHighlightPerUser { get; set; }
        public bool OnlyHighlightOnce { get; set; }
        public bool Editable { get; set; }
        public List<TopicShowModel>? MovieTopics { get; set; }

    }
}