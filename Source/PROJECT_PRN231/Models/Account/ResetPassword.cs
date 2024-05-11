using System.ComponentModel.DataAnnotations;

namespace PROJECT_PRN231.Models.Account
{
    public class ResetPassword
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
