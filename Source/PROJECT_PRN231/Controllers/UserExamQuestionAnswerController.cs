using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;

namespace PROJECT_PRN231.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserExamQuestionAnswerController : ControllerBase
    {
        private readonly IUserExamQuestionAnswerRepository _userExamQuestionAnswerRepository;
        public UserExamQuestionAnswerController(IUserExamQuestionAnswerRepository userExamQuestionAnswerRepository)
        {
            _userExamQuestionAnswerRepository = userExamQuestionAnswerRepository;
        }

		//[Authorize(Roles = "ADMIN")]
		[HttpGet]
        public IActionResult GetAll()
        {
            var list = _userExamQuestionAnswerRepository.GetAll();
            if(list == null)
            {
                return NotFound("there is no record");
            }
            return Ok(list);
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpPost("SelectAnswer")]
        public IActionResult SelectAnswer([FromBody]UserExamQuestionAnswerVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var selectionExist = _userExamQuestionAnswerRepository.IsQuestionAnswered(viewModel.QuestionId.Value, viewModel.ExamId.Value, viewModel.UserId.Value);
            if (selectionExist == null)
            {
                var model = new UserExamQuestionAnswer
                {
                    ExamId = viewModel.ExamId,
                    QuestionId = viewModel.QuestionId,
                    UserId = viewModel.UserId,
                    IsCorrect = viewModel.IsCorrect,
                    AnswerId = viewModel.AnswerId
                };
                if (_userExamQuestionAnswerRepository.AddUserExamQuestionAnswer(model))
                {
                    return Ok("Selection added");
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong while creating new record");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                selectionExist.AnswerId = viewModel.AnswerId;
                selectionExist.IsCorrect = viewModel.IsCorrect;
                if (_userExamQuestionAnswerRepository.UpdateUserExamQuestionAnswer(selectionExist))
                {
                    return Ok("Selection updated");
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong while updating record");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}
