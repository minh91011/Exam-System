using PROJECT_PRN231.Models;

namespace PROJECT_PRN231.Interface
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetById(int id);
        User GetByUserName(string userName);
        User GetByEmail(string email);
        bool UpdateUser(User user);
        bool DeleteUser(int id);
        bool AddUser(User user);
        bool Save();
    }
}
