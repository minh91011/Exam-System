using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PROJECT_PRN231.Models
{
    public partial class UserExamResult
    {
        [Required]
        public int ResultId { get; set; }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public int? ExamId { get; set; }
        [Required]
        public double? Score { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual Exam? Exam { get; set; }
        public virtual User? User { get; set; }
    }
}
