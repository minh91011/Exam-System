using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PROJECT_PRN231.Models
{
	public partial class UserExamQuestionAnswer
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
		[Required]
		public int Id { get; set; }
		[IgnoreDataMember]
		public virtual Answer? Answer { get; set; }
		[IgnoreDataMember]
		public virtual Exam? Exam { get; set; }
		[IgnoreDataMember]
		public virtual Question? Question { get; set; }
		[IgnoreDataMember]
		public virtual User? User { get; set; }
	}
}
