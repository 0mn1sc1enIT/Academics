using System.ComponentModel.DataAnnotations;

namespace Academics.Models
{
    public class NewsletterSignUp
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}
