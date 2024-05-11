using System.ComponentModel.DataAnnotations;

namespace PROJECT_PRN231.Models.ViewModel
{
    public class UserExamQuestionAnswerVM
    {
        [Required]
        public int? UserId { get; set; }
        [Required]
        public int? ExamId { get; set; }
        [Required]
        public int? QuestionId { get; set; }
        [Required]
        public int? AnswerId { get; set; }
        [Required]
        public bool? IsCorrect { get; set; }
    }
}
