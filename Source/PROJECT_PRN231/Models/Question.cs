using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PROJECT_PRN231.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
            ExamQuestions = new HashSet<ExamQuestion>();
            UserExamQuestionAnswers = new HashSet<UserExamQuestionAnswer>();
        }

        public int QuestionId { get; set; }
        public string? QuestionText { get; set; }
        public int? DifficultyLevel { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        [IgnoreDataMember]
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        [IgnoreDataMember]
        public virtual ICollection<UserExamQuestionAnswer> UserExamQuestionAnswers { get; set; }
    }
}
