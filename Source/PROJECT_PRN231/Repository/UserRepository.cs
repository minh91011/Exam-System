using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;

namespace PROJECT_PRN231.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ExamSystemContext _context;
        public UserRepository(ExamSystemContext context)
        {
            _context = context;
        }
        public bool AddUser(User user)
        {
            _context.Users.Add(user);
            return Save();
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            return Save();
        }

        public List<User> GetAll()
        {
            var list = _context.Users.ToList();
            return list;
        }

        public User GetByEmail(string email)
        {
            var user = _context.Users.Where(x => x.Email == email && x.Verified == true).FirstOrDefault();
            return user;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User GetByUserName(string userName)
        {
            return _context.Users.Where(x => x.Username == userName).FirstOrDefault();
        }

        public bool Save()
        {
            int save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            var userExist = GetByUserName(user.Username);
            if(userExist != null)
            {
                _context.Users.Update(user);
            }
            return Save();
        }
    }
}
