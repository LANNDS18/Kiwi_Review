using System.ComponentModel.DataAnnotations;

namespace Kiwi_review.Models.AddingDto
{
    public class UserAddingModel
    {

        [Required]
        [MinLength(2)]
        [Display(Name = "Alias")]
        public string? Alias { get; set; }
        
        [Required]
        [MinLength(2)]
        [Display(Name = "Description")]
        public string? Description { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}