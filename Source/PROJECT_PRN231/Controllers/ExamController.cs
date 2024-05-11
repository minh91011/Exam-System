using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.ViewModel;

namespace PROJECT_PRN231.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        public IExamRepository _ExamRepository;
        private ExamSystemContext _examSystemContext;
        public ExamController(IExamRepository examRepository)
        {
            _ExamRepository = examRepository;
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_ExamRepository.GetAll());
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
                var find = _ExamRepository.GetById(id);
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
        public IActionResult Create(ExamVM exam)
        {
            try
            {
                return Ok(_ExamRepository.Create(exam));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpPut("Update/{id}")]
        public ActionResult Update(int id, Exam exam)
        {
            if (id != exam.ExamId)
            {
                return NotFound("Not found!");
            }
            try
            {
                _ExamRepository.Update(exam);
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
            var find = _ExamRepository.GetById(id);
            if (find == null)
            {
                return NotFound("Not found!");
            }
            try
            {
                _ExamRepository.Delete(id);
                return Ok("Delete success!");
            }
            catch
            {
                return BadRequest("Exam is referenced in UserExamResult. Cannot delete!");
            }
        }

    }
}
