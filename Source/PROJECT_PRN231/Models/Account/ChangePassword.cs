using System.ComponentModel.DataAnnotations;

namespace PROJECT_PRN231.Models.Account
{
    public class ChangePassword
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 8 , ErrorMessage = "Password must have at least 8 characters")]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "Confirm password dont match with password")]
        public string ConfirmNewPassword { get; set; }
    }
}
