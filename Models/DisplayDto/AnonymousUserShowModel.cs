using Kiwi_review.Models.DatabaseModel;
using Kiwi_review.Models.Jwt;

namespace Kiwi_review.Models.DisplayDto
{
    public class AnonymousUserShowModel : BaseEntity
    {
        public int UserId { get; set; }
        public bool IsAnonymousUser { get; set; }
        public TnToken? JwtToken { get; set; }
    }
}