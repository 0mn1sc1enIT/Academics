using System.ComponentModel.DataAnnotations;

namespace Academics.Models
{
    public class Login
    {
        [Required(ErrorMessage = "field")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "field")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
