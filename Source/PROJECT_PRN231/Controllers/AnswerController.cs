using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models.ViewModel;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Repository;
using Microsoft.AspNetCore.Authorization;

namespace PROJECT_PRN231.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        public IAnswerRepository AnswerRepository;
        public AnswerController(IAnswerRepository answerRepository)
        {
            AnswerRepository = answerRepository;
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(AnswerRepository.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpGet("id")]
        public IActionResult Get(int id)
        {
            try
            {
                var find = AnswerRepository.GetById(id);
                if (find == null)
                {
                    return NotFound("Not found!");
                }
                return Ok(find);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpGet("GetByQuestionId/id")]
        public IActionResult GetByQuestionId(int id)
        {
            try
            {
                var find = AnswerRepository.GetByQuestionId(id);
                if (find == null)
                {
                    return NotFound("Not found!");
                }
                return Ok(find);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpPost("Add")]
        public IActionResult Create(AnswerVM answerVM)
        {
            try
            {
                return Ok(AnswerRepository.Create(answerVM));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpPut("Update/{id}")]
        public ActionResult Update(int id, AnswerVM answerVM)
        {
            try
            {
                AnswerRepository.Update(id, answerVM);
                return Ok("Update success!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            var find = AnswerRepository.GetById(id);
            if (find == null)
            {
                return NotFound("Not found!");
            }
            try
            {
                AnswerRepository.Delete(id);
                return Ok("Delete success");
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
