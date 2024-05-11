using PROJECT_PRN231.Models.ViewModel;
using PROJECT_PRN231.Models;

namespace PROJECT_PRN231.Interface
{
    public interface IQuestionRepository
    {
        List<Question> GetAll();
        Question GetById(int id);
        Question Create(QuestionVM questionVM);
        void Update(Question question);
        void Delete(int id);
    }
}
