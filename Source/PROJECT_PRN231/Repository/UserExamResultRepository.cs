using Microsoft.EntityFrameworkCore;
using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;

namespace PROJECT_PRN231.Repository
{
	public class UserExamResultRepository : IUserExamResultRepository
	{
		private ExamSystemContext _context;
		public UserExamResultRepository(ExamSystemContext context)
		{
			_context = context;
		}

		public bool AddUserExamResult(UserExamResult userExamResult)
		{
			_context.UserExamResults.Add(userExamResult);
			return Save();
		}

		public bool DeleteUserExamResult(int id)
		{
			var userExamResult = _context.UserExamResults.Find(id);
			if (userExamResult != null)
			{
				_context.UserExamResults.Remove(userExamResult);
			}
			return Save();
		}

		public List<UserExamResultVM> GetAll()
		{
			var list = _context.UserExamResults.ToList();
			if (list.Count == 0)
			{
				return null;
			}
			var listVM = new List<UserExamResultVM>();
			foreach (var item in list)
			{
				listVM.Add(new UserExamResultVM
				{
					UserId = item.UserId,
					ExamId = item.ExamId,
					Score = item.Score,
					StartTime = item.StartTime,
					EndTime = item.EndTime
				});
			}
			return listVM;
		}

		public UserExamResult GetById(int id)
		{
			return _context.UserExamResults.Find(id);
		}

		public List<UserExamResult> GetByUserId(int userId)
		{
			return _context.UserExamResults.Where(x => x.UserId == userId).Include(x => x.Exam).OrderByDescending(x => x.ExamId).OrderByDescending(x => x.StartTime).ToList();
			//if (list.Count == 0)
			//{
			//    return null;
			//}
			//var listVM = new List<UserExamResultVM>();
			//foreach (var item in list)
			//{
			//    listVM.Add(new UserExamResultVM
			//    {
			//        UserId = item.UserId,
			//        ExamId = item.ExamId,
			//        Score = item.Score,
			//        StartTime = item.StartTime,
			//        EndTime = item.EndTime
			//    });
			//}
			//return listVM;
		}

		public UserExamResult GetPendingResult(int userId, int examId)
		{
			var pendingResult = _context.UserExamResults.Where(x => x.UserId == userId && x.ExamId == examId && x.EndTime == null).Include(x => x.Exam).FirstOrDefault();
			return pendingResult;
		}

		public bool Save()
		{
			int save = _context.SaveChanges();
			return save > 0 ? true : false;
		}

		public bool UpdateUserExamResult(UserExamResult userExamResult)
		{
			_context.UserExamResults.Update(userExamResult);
			return Save();
		}




		//public UserExamResult Create(UserExamResultVM userExamResultVM)
		//{
		//    var _new = new UserExamResult
		//    {
		//        UserId = userExamResultVM.UserId,
		//        ExamId = userExamResultVM.ExamId,
		//        Score = userExamResultVM.Score,
		//        StartTime = userExamResultVM.StartTime,
		//        EndTime = userExamResultVM.EndTime,
		//    };
		//    _context.Add(_new);
		//    _context.SaveChanges();

		//    return new UserExamResult
		//    {
		//        ResultId = _new.ResultId,
		//        UserId = userExamResultVM.UserId,
		//        ExamId = userExamResultVM.ExamId,
		//        Score = userExamResultVM.Score,
		//        StartTime = userExamResultVM.StartTime,
		//        EndTime = userExamResultVM.EndTime,
		//    };
		//}

		//public void Delete(int id)
		//{
		//    var find = _context.UserExamResults.SingleOrDefault(f => f.ResultId == id); 
		//    if (find != null)
		//    {
		//        _context.Remove(find);
		//        _context.SaveChanges();
		//    }
		//}

		//public List<UserExamResult> GetAll()
		//{
		//    var list = _context.UserExamResults.ToList();
		//    return list;
		//}

		//public UserExamResult GetById(int id)
		//{
		//    var find = _context.UserExamResults.FirstOrDefault(f => f.ResultId == id);
		//    if (find != null)
		//    {
		//        return new UserExamResult
		//        {
		//            ResultId= find.ResultId,
		//            UserId = find.UserId,   
		//            ExamId = find.ExamId,
		//            Score = find.Score,
		//            StartTime = find.StartTime,
		//            EndTime = find.EndTime,
		//        };
		//    }
		//    return null;
		//}

		//public void Update(UserExamResult userExamResult)
		//{
		//    throw new NotImplementedException();
		//}
	}
}
