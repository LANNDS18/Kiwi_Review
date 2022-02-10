namespace Kiwi_review.Models.DatabaseModel
{
    public class Topic : BaseEntity
    {
        public int TopicId { get; set; }
        public string? TopicName { get; set; }
        public int MovieId { get; set; }
    }
}