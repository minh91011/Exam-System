using PROJECT_PRN231.Models.ViewModel;
using PROJECT_PRN231.Models;

namespace PROJECT_PRN231.Interface
{
    public interface IExamQuestionRepository
    {
        List<ExamQuestion> GetAll();
        ExamQuestion GetById(int id);
        List<ExamQuestion> GetAllQuestionsOfExam(int examId);
        ExamQuestion Create(ExamQuestionVM examQuestionVM);
        void Update(ExamQuestion examQuestionVM);
        void Delete(int id);
    }
}
