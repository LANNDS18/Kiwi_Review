using System;

namespace Kiwi_review.Models.Jwt
{
    public class TnToken
    {
        public string? TokenStr { get; set; }
        public DateTime Expires { get; set; }
    }
}