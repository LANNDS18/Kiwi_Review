using Kiwi_review.Models.DatabaseModel;
using Kiwi_review.Models.Jwt;

namespace Kiwi_review.Models.DisplayDto
{
    public class UserShowModel : BaseEntity
    {
        public int UserId { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public bool IsAnonymousUser { get; set; }
        public TnToken? JwtToken { get; set; }
    }
}