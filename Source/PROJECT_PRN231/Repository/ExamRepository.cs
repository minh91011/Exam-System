using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;

namespace PROJECT_PRN231.Repository
{
    public class ExamRepository : IExamRepository
    {

        private ExamSystemContext _context;
        public ExamRepository(ExamSystemContext context)
        {
            _context = context;
        }

        public bool checkExamId(int id)
        {
            //// Kiểm tra xem có dữ liệu liên quan trong bảng UserExamResult hay không
            //var examHasResults = _context.UserExamResults.Any(uer => uer.ExamId == id);
            //if (examHasResults)
            //{
            //    return BadRequest("Exam is referenced in UserExamResult. Cannot delete!");
            //}
            return _context.UserExamResults.Any(uer => uer.ExamId == id);
        }

        public Exam Create(ExamVM exam)
        {
            var _new = new Exam
            {
                ExamName = exam.ExamName,
                Duration = exam.Duration
            };
            _context.Add(_new);
            _context.SaveChanges();
            return new Exam
            {
                ExamId = _new.ExamId,
                ExamName = exam.ExamName,
                Duration = exam.Duration,
            };
        }

        public void Delete(int id)
        {
            var find = _context.Exams.SingleOrDefault(e => e.ExamId  == id); 
            if (find != null)
            {
                _context.Exams.Remove(find);
                _context.SaveChanges();
            }
        }

        public List<Exam> GetAll()
        {
            var list = _context.Exams.ToList();
            return list;
        }

        public Exam GetById(int id)
        {
            var find = _context.Exams.FirstOrDefault(e => e.ExamId == id);
            if(find != null)
            {
                return new Exam
                {
                    ExamId = find.ExamId,
                    ExamName = find.ExamName,
                    Duration = find.Duration,
                };
            }
            return null;
        }

        public int GetQuestionCount(int id)
        {
            return _context.ExamQuestions.Where(x => x.ExamId == id).Count();
        }

        public void Update(Exam exam)
        {
            var find = _context.Exams.SingleOrDefault(e => e.ExamId == exam.ExamId);
            if(find != null)
            {
                find.ExamName = exam.ExamName;
                find.Duration = exam.Duration;
                _context.SaveChanges();
            }
        }
    }
}
