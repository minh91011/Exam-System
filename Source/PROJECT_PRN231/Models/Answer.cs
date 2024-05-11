using System;
using System.Collections.Generic;

namespace PROJECT_PRN231.Models
{
    public partial class Answer
    {
        public Answer()
        {
            UserExamQuestionAnswers = new HashSet<UserExamQuestionAnswer>();
        }

        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string? Value { get; set; }
        public bool? IsCorrect { get; set; }

        public virtual Question Question { get; set; } = null!;
        public virtual ICollection<UserExamQuestionAnswer> UserExamQuestionAnswers { get; set; }
    }
}
