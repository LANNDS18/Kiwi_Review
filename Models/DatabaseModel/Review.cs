namespace Kiwi_review.Models.DatabaseModel
{
    public class Review : BaseEntity
    {
        public int ReviewId { get; set; }
        public string? ReviewContent { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int TopicId { get; set; }
    }
}