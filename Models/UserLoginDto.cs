using System.ComponentModel.DataAnnotations;

namespace Kiwi_review.Models
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

    }
}