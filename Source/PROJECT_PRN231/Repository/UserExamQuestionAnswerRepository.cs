using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;

namespace PROJECT_PRN231.Repository
{
    public class UserExamQuestionAnswerRepository : IUserExamQuestionAnswerRepository
    {
        private readonly ExamSystemContext _context;
        public UserExamQuestionAnswerRepository(ExamSystemContext context)
        {
            _context = context;
        }

        public bool AddUserExamQuestionAnswer(UserExamQuestionAnswer userExamQuestionAnswer)
        {
            _context.UserExamQuestionAnswers.Add(userExamQuestionAnswer);
            return Save();
        }

        public bool DeleteUserExamQuestionAnswer(int id)
        {
            _context.UserExamQuestionAnswers.Remove(GetById(id));
            return Save();
        }

        public bool DeleteUserExamQuestionAnswer(List<UserExamQuestionAnswer> list)
        {
            _context.UserExamQuestionAnswers.RemoveRange(list);
            return Save();
        }


		public List<UserExamQuestionAnswerVM> GetAll()
        {
            var list = _context.UserExamQuestionAnswers.ToList();
            if (list == null)
            {
                return null;
            }
            var listVM = new List<UserExamQuestionAnswerVM>();
            foreach (var item in list)
            {
                listVM.Add(new UserExamQuestionAnswerVM
                {
                    AnswerId = item.AnswerId,
                    ExamId = item.ExamId,
                    IsCorrect = item.IsCorrect,
                    QuestionId = item.QuestionId,
                    UserId = item.UserId
                });
            }
            return listVM;
        }

        public List<UserExamQuestionAnswer> GetAllUserAnswerInExam(int userId, int examId)
        {
            return _context.UserExamQuestionAnswers.Where(x => x.UserId == userId && x.ExamId == examId).ToList();
            //if (list == null)
            //{
            //    return null;
            //}
            //var listVM = new List<UserExamQuestionAnswerVM>();
            //foreach (var item in list)
            //{
            //    listVM.Add(new UserExamQuestionAnswerVM
            //    {
            //        AnswerId = item.AnswerId,
            //        ExamId = item.ExamId,
            //        IsCorrect = item.IsCorrect,
            //        QuestionId = item.QuestionId,
            //        UserId = item.UserId
            //    });
            //}
            //return listVM;
        }

        public UserExamQuestionAnswer GetById(int id)
        {
            return _context.UserExamQuestionAnswers.Find(id);
        }

        public UserExamQuestionAnswer IsQuestionAnswered(int questionId, int examId, int userId)
        {
            var exist = _context.UserExamQuestionAnswers.Where(x=> x.UserId == userId && x.ExamId == examId && x.QuestionId == questionId).FirstOrDefault();
            return exist;
        }

        public bool Save()
        {
            int save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateUserExamQuestionAnswer(UserExamQuestionAnswer userExamQuestionAnswer)
        {
            _context.Update(userExamQuestionAnswer);
            return Save();
        }
    }
}
