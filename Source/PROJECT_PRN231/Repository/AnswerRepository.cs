using Microsoft.EntityFrameworkCore;
using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;

namespace PROJECT_PRN231.Repository
{
    public class AnswerRepository : IAnswerRepository
    {
        private ExamSystemContext _context;

        public AnswerRepository(ExamSystemContext context)
        {
            _context = context;
        }
        public Answer Create(AnswerVM answerVM)
        {
            var _new = new Answer
            {
                QuestionId = answerVM.QuestionId,
                Value = answerVM.Value,
                IsCorrect = answerVM.IsCorrect,
            };
            _context.Answers.Add(_new);
            _context.SaveChanges();
            return new Answer
            {
                AnswerId = _new.AnswerId,
                QuestionId = answerVM.QuestionId,
                Value = answerVM.Value,
                IsCorrect = answerVM.IsCorrect,
            };
        }

        public void Delete(int id)
        {
            var find = _context.Answers.SingleOrDefault(f => f.AnswerId == id);
            if (find != null)
            {
                _context.Answers.Remove(find);
                _context.SaveChanges();
            }
        }

        public List<Answer> GetAll()
        {
            var list = _context.Answers.ToList();
            return list;
        }

        public Answer GetById(int id)
        {
            var find = _context.Answers.FirstOrDefault(f => f.AnswerId == id);
            if (find != null)
            {
                return new Answer
                {
                    AnswerId = find.AnswerId,
                    QuestionId = find.QuestionId,
                    Value = find.Value,
                    IsCorrect = find.IsCorrect,
                };
            }
            return null;
        }

        public List<Answer> GetByQuestionId(int id)
        {
            var answers = _context.Answers.Where(a => a.QuestionId == id).ToList();

            if (answers != null && answers.Any())
            {
                return answers.Select(answer => new Answer
                {
                    AnswerId = answer.AnswerId,
                    QuestionId = answer.QuestionId,
                    Value = answer.Value,
                    IsCorrect = answer.IsCorrect
                }).ToList();
            }

            return null;
        }


        public void Update(int id, AnswerVM answerVM)
        {
            var find = _context.Answers.FirstOrDefault(f => f.AnswerId == id);
            if (find != null)
            {
                find.QuestionId = answerVM.QuestionId;
                find.Value = answerVM.Value;
                find.IsCorrect = answerVM.IsCorrect;
                _context.SaveChanges();
            }
        }
    }
}
