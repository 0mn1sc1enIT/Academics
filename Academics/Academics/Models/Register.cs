using System.ComponentModel.DataAnnotations;

namespace Academics.Models
{
    public class Register
    {
        [Required(ErrorMessage = "field")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "field")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "field")]
        [Compare("Password", ErrorMessage = "match")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
