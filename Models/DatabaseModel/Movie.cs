namespace Kiwi_review.Models.DatabaseModel
{
    public class Movie : BaseEntity
    {
        public int MovieId { get; set; }
        public string? MovieDescription { get; set; }
        public int UserId { get; set; }
        public bool AllowAnonymous { get; set; }
        public int NumberTopic { get; set; }
        public int MaxHighlightPerUser { get; set; }
        public bool OnlyHighlightOnce { get; set; }
    }
}