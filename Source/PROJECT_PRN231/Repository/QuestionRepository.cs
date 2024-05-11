using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;

namespace PROJECT_PRN231.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private ExamSystemContext _context;

        public QuestionRepository(ExamSystemContext context)
        {
            _context = context;
        }

        public Question Create(QuestionVM questionVM)
        {
            var _new = new Question
            {
                QuestionText = questionVM.QuestionText,
                DifficultyLevel = questionVM.DifficultyLevel,
            };
            _context.Questions.Add(_new);
            _context.SaveChanges();
            return new Question
            {
                QuestionId = _new.QuestionId,
                QuestionText = questionVM.QuestionText,
                DifficultyLevel = questionVM.DifficultyLevel,
            };
        }

        public void Delete(int id)
        {
            var find = _context.Questions.SingleOrDefault(f=>f.QuestionId == id);
            if (find != null)
            {
                _context.Questions.Remove(find);
                _context.SaveChanges();
            }
        }

        public List<Question> GetAll()
        {
            var list = _context.Questions.ToList();
            return list;
        }

        public Question GetById(int id)
        {
            var find = _context.Questions.FirstOrDefault(f => f.QuestionId == id);
            if (find != null)
            {
                return new Question
                {
                    QuestionId = find.QuestionId,
                    QuestionText = find.QuestionText,
                    DifficultyLevel = find.DifficultyLevel,
                };
            }
            return null;
        }

        public void Update(Question question)
        {
            var find = _context.Questions.FirstOrDefault(f => f.QuestionId == question.QuestionId);
            if (find != null)
            {
                find.QuestionText = question.QuestionText;
                find.DifficultyLevel = question.DifficultyLevel;
                _context.SaveChanges();
            }
        }
    }
}
