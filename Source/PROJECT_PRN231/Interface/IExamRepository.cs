using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;

namespace PROJECT_PRN231.Interface
{
    public interface IExamRepository
    {
        List<Exam> GetAll();
        Exam GetById(int id);
        Exam Create(ExamVM examVM);
        void Update(Exam exam);
        void Delete(int id);
        int GetQuestionCount(int id);

        public bool checkExamId(int id);
    }
}
