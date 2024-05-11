using Microsoft.EntityFrameworkCore;
using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;

namespace PROJECT_PRN231.Repository
{
    public class ExamQuestionRepository : IExamQuestionRepository
    {
        private ExamSystemContext _context;
        public ExamQuestionRepository(ExamSystemContext context)
        {
            _context = context;
        }
        public ExamQuestion Create(ExamQuestionVM examQuestionVM)
        {
            var _new = new ExamQuestion
            {
                ExamId = examQuestionVM.ExamId,
                QuestionId = examQuestionVM.QuestionId,
                QuestionOrder = examQuestionVM.QuestionOrder,
            };
            _context.Add(_new);
            _context.SaveChanges();
            return new ExamQuestion
            {
                ExamQuestionId = _new.ExamQuestionId,
                ExamId = examQuestionVM.ExamId,
                QuestionId = examQuestionVM.QuestionId,
                QuestionOrder = examQuestionVM.QuestionOrder,
            };
        }

        public void Delete(int id)
        {
            var find = _context.ExamQuestions.SingleOrDefault(f => f.ExamQuestionId == id);
            if (find != null)
            {
                _context.ExamQuestions.Remove(find);
                _context.SaveChanges();
            }
        }

        public List<ExamQuestion> GetAll()
        {
            var list = _context.ExamQuestions.ToList();
            return list;
        }

        public List<ExamQuestion> GetAllQuestionsOfExam(int examId)
        {
            var list = _context.ExamQuestions.Where(x => x.ExamId == examId).Include(x => x.Question).ThenInclude(x => x.Answers).ThenInclude(x => x.UserExamQuestionAnswers).ToList();
            return list;
        }

        public ExamQuestion GetById(int id)
        {
            var find = _context.ExamQuestions.FirstOrDefault(e => e.ExamQuestionId == id);
            if (find != null)
            {
                return new ExamQuestion
                {
                    ExamQuestionId = find.ExamQuestionId,
                    ExamId = find.ExamId,
                    QuestionId = find.QuestionId,
                    QuestionOrder = find.QuestionOrder,
                };
            }
            return null;
        }

        public void Update(ExamQuestion examQuestion)
        {
            var find = _context.ExamQuestions.FirstOrDefault(f => f.ExamQuestionId == examQuestion.ExamQuestionId);
            if (find != null)
            {
                find.ExamId = examQuestion.ExamId;
                find.QuestionId = examQuestion.QuestionId;
                find.QuestionOrder = examQuestion.QuestionOrder;
                _context.SaveChanges();
            }
        }
    }
}
