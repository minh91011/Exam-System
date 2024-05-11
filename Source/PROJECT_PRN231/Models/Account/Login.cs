using System.ComponentModel.DataAnnotations;

namespace PROJECT_PRN231.Models.Account
{
    public class Login
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
