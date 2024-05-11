using System.ComponentModel.DataAnnotations;

namespace PROJECT_PRN231.Models.Account
{
    public class Register
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 8, ErrorMessage = "Password must have at least 8 characters")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Confirm password dont match with password")]
        public string ConfirmPassword { get; set; }
    }
}
