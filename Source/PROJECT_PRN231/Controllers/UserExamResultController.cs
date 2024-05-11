using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;
using PROJECT_PRN231.Repository;

namespace PROJECT_PRN231.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserExamResultController : ControllerBase
    {
        private readonly IUserExamResultRepository _userExamResultRepository;
        private readonly IUserExamQuestionAnswerRepository _userExamQuestionAnswerRepository;
        private readonly IExamRepository _examRepository;
        public UserExamResultController(IUserExamResultRepository userExamResultRepository,
            IUserExamQuestionAnswerRepository userExamQuestionAnswerRepository,
            IExamRepository examRepository)
        {
            _userExamResultRepository = userExamResultRepository;
            _userExamQuestionAnswerRepository = userExamQuestionAnswerRepository;
            _examRepository = examRepository;
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _userExamResultRepository.GetAll();
            if (list == null)
            {
                return NotFound("There is no result");
            }
            return Ok(list);

        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpGet("id/{id}")]
        public IActionResult GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _userExamResultRepository.GetById(id);
            if (result == null)
            {
                return NotFound($"No result with id {id}");
            }
            return Ok(result);
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpGet("userId/{userId}")]
        public IActionResult GetByUser(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var list = _userExamResultRepository.GetByUserId(userId);
            if (list == null)
            {
                return NotFound($"User with id {userId} have no result");
            }
            return Ok(list);
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpPost]
        public IActionResult Create([FromBody] UserExamResultVM userExamResultVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pendingResult = _userExamResultRepository.GetPendingResult(userExamResultVM.UserId.Value, userExamResultVM.ExamId.Value);
            if (pendingResult != null)
            {
                Update(userExamResultVM.UserId.Value, userExamResultVM.ExamId.Value);
            }
            var userExamResult = new UserExamResult
            {
                UserId = userExamResultVM.UserId,
                ExamId = userExamResultVM.ExamId,
                Score = userExamResultVM.Score,
                StartTime = userExamResultVM.StartTime,
                EndTime = userExamResultVM.EndTime
            };
            if (_userExamResultRepository.AddUserExamResult(userExamResult))
            {
                return Ok(userExamResult);
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong while creating new record");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpPut("{userId}/{examId}")]
        public IActionResult Update(int userId, int examId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pendingResult = _userExamResultRepository.GetPendingResult(userId, examId);
            if (pendingResult == null)
            {
                return NotFound($"Result of userId {userId} with examId {examId} not found");
            }
            var examAnswers = _userExamQuestionAnswerRepository.GetAllUserAnswerInExam(pendingResult.UserId.Value, pendingResult.ExamId.Value);
            var correctAnswers = examAnswers.Where(x => x.IsCorrect == true).Count();
            var answersCount = _examRepository.GetQuestionCount(pendingResult.ExamId.Value);
            pendingResult.Score = (10d / answersCount) * correctAnswers;
            pendingResult.EndTime = DateTime.Now;
            if (!_userExamQuestionAnswerRepository.DeleteUserExamQuestionAnswer(examAnswers))
            {
                ModelState.AddModelError("", "Something went wrong while deleting record");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (_userExamResultRepository.UpdateUserExamResult(pendingResult))
            {
                return Ok(pendingResult);
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong while updating record");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var find = _userExamResultRepository.GetById(id);
            if (find == null)
            {
                return NotFound("Not found!");
            }
            if (_userExamResultRepository.DeleteUserExamResult(id))
            {
                return Ok("Result deleted");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong while deleting new record");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
