using System.ComponentModel.DataAnnotations;

namespace Academics.Models
{
    public class Message
    {
        [Required(ErrorMessage = "field")]
        [NameValidate()]
        [Display(Name = "firstName")]
        public string firstName { get; set; }

        [NameValidate()]
        [Display(Name = "lastName")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "field")]
        [EmailAddress(ErrorMessage = "email_error")]
        [Display(Name = "email")]
        public string email { get; set; }

        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "phone_error")]
        [Display(Name = "phone")]
        public string phoneNumber { get; set; }
        [Display(Name = "message")]
        public string message { get; set; }
    }
}
