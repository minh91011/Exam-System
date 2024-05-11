using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.Account;
using PROJECT_PRN231.Models.Mail;
using PROJECT_PRN231.Utilities;
using System.Security.Cryptography;

namespace PROJECT_PRN231.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AppSettings _applicationSettings;
        private readonly IUserRepository _userRepository;
        public UserController(IOptions<AppSettings> applicationSettings, IUserRepository userRepository)
        {
            _applicationSettings = applicationSettings.Value;
            _userRepository = userRepository;
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpGet("GetAll")]
        public IActionResult GetAllUsers()
        {
            var list = _userRepository.GetAll();
            if (list == null)
            {
                return NotFound("User table is empty");
            }
            return Ok(list);
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpGet("Detail/{username}")]
        public IActionResult GetByUsername(string username)
        {
            var user = _userRepository.GetByUserName(username);
            if (user == null)
            {
                return NotFound("User id not found");
            }
            return Ok(user);

        }



        //[Authorize(Roles = "ADMIN,USER")]
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userExist = _userRepository.GetByUserName(model.Username);
            if (userExist == null)
            {
                return NotFound("User not exist");
            }
            if (!CheckPassword(model.OldPassword, userExist))
            {
                return BadRequest("Invalid old password");
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return BadRequest("New password and Confirm new password not correct");
            }
            using (HMACSHA512? hmac = new HMACSHA512())
            {
                userExist.PasswordSalt = hmac.Key;
                userExist.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.NewPassword));
            }
            if (_userRepository.UpdateUser(userExist))
            {
                return Ok("User's password changed");
            }
            else
            {
                ModelState.AddModelError("", "Error when change password");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userRepository.GetByEmail(resetPassword.Email);
            if (user == null)
            {
                return NotFound("Email dont exist");
            }

            MailHelper mailHelper = new MailHelper();
            var newPassword = await mailHelper.PostMailResetPasswordAsync(resetPassword.Email);
            if (newPassword == "failed")
            {
                ModelState.AddModelError("", "Error when send OTP to mail");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            using (HMACSHA512? hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newPassword));
            }
            if (!_userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Error when saving new password to database");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok("Password reseted, please check your email for futher information");

        }

        //[Authorize(Roles = "ADMIN")]
        [HttpDelete("DeleteUser")]
        public IActionResult Delete([FromBody] string userName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userRepository.GetByUserName(userName);
            if (user == null)
            {
                return NotFound();
            }
            if (!_userRepository.DeleteUser(user.UserId))
            {
                ModelState.AddModelError("", "Error when delete user");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return Ok(user);
            }
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpPost("SendMailOTP")]
        public async Task<IActionResult> SendMailOTP([FromBody] SendOTP model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var emailExist = _userRepository.GetByEmail(model.Email);
            if (emailExist != null)
            {
                return BadRequest("Email already registered");
            }

            var user = _userRepository.GetByUserName(model.Username);
            if (user == null)
            {
                return NotFound("User not found");
            }

            //if (user.Email != null)
            //{
            //    return BadRequest("User already have email");
            //}

            MailHelper mailHelper = new MailHelper();
            string otp = await mailHelper.PostMailOTPAsync(model.Email);
            if (otp == "failed")
            {
                ModelState.AddModelError("", "Error when send OTP to mail");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            user.OtpCode = otp;
            user.Verified = false;
            if (!_userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Error when saving otp to database");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = new OTPResponse
            {
                Username = model.Username,
                Email = model.Email
            };

            return Ok(response);
        }

        //[Authorize(Roles = "ADMIN,USER")]
        [HttpPost("ConfirmOTP")]
        public IActionResult ConfirmOTP([FromBody] OTPResponse model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userRepository.GetByUserName(model.Username);
            if (user == null)
            {
                return NotFound();
            }
            if (user.OtpCode == null)
            {
                return BadRequest("The system did'nt send any otp code to this email");
            }
            //if (user.Email != model.Email)
            //{
            //    return BadRequest("wrong email");
            //}
            if (model.OTPCode != user.OtpCode)
            {
                return BadRequest("Otp code invalid");
            }
            user.Email = model.Email;
            user.Verified = true;
            user.OtpCode = null;
            if (!_userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Error when verifying user in database");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok("User successfully verified");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public bool CheckPassword(string password, User user)
        {
            bool result;
            using (HMACSHA512? hmac = new HMACSHA512(user.PasswordSalt))
            {
                var compute = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                result = compute.SequenceEqual(user.PasswordHash);
            }
            return result;
        }
    }
}
