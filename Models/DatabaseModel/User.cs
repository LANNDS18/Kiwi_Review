namespace Kiwi_review.Models.DatabaseModel
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsAnonymousUser { get; set; }
    }
}