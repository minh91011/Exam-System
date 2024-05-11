using System;
using System.Collections.Generic;

namespace PROJECT_PRN231.Models
{
    public partial class ExamQuestion
    {
        public int ExamQuestionId { get; set; }
        public int? ExamId { get; set; }
        public int? QuestionId { get; set; }
        public int? QuestionOrder { get; set; }

        public virtual Exam? Exam { get; set; }
        public virtual Question? Question { get; set; }
    }
}
