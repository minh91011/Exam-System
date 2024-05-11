using System.ComponentModel.DataAnnotations;

namespace PROJECT_PRN231.Models.Mail
{
    public class OTPResponse
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Please enter a 5-digit number.")]
        public string OTPCode { get; set; }
    }
}
