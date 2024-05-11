using System;
using System.Collections.Generic;

namespace PROJECT_PRN231.Models
{
    public partial class User
    {
        public User()
        {
            UserExamQuestionAnswers = new HashSet<UserExamQuestionAnswer>();
            UserExamResults = new HashSet<UserExamResult>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? OtpCode { get; set; }
        public bool Verified { get; set; }

        public virtual ICollection<UserExamQuestionAnswer> UserExamQuestionAnswers { get; set; }
        public virtual ICollection<UserExamResult> UserExamResults { get; set; }
    }
}
