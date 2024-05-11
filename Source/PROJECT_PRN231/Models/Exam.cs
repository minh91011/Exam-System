using System;
using System.Collections.Generic;

namespace PROJECT_PRN231.Models
{
    public partial class Exam
    {
        public Exam()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
            UserExamQuestionAnswers = new HashSet<UserExamQuestionAnswer>();
            UserExamResults = new HashSet<UserExamResult>();
        }

        public int ExamId { get; set; }
        public string? ExamName { get; set; }
        public int? Duration { get; set; }

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<UserExamQuestionAnswer> UserExamQuestionAnswers { get; set; }
        public virtual ICollection<UserExamResult> UserExamResults { get; set; }
    }
}
