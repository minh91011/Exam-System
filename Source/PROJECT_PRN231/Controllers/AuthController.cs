using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using PROJECT_PRN231.Interface;
using PROJECT_PRN231.Models;
using PROJECT_PRN231.Models.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PROJECT_PRN231.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppSettings _applicationSettings;
        private readonly IUserRepository _userRepository;
        public AuthController(IOptions<AppSettings> applicationSettings, IUserRepository userRepository)
        {
            //hahahahahah
            _applicationSettings = applicationSettings.Value;
            _userRepository = userRepository;
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login login)
        {
            var user = _userRepository.GetByUserName(login.Username);
            if (user == null)
            {
                return BadRequest("UserName or password incorrect");
            }
            var match = CheckPassword(login.Password, user);
            if (!match)
            {
                return BadRequest("UserName or password incorrect");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._applicationSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", user.Username), new Claim(ClaimTypes.Role, user.Role.ToUpper()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encrypterToken = tokenHandler.WriteToken(token);

            var result = new LoginResult
            {
                Id = user.UserId,
                Username = user.Username,
                Token = encrypterToken
            };
            return Ok(result);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool CheckPassword(string password, User user)
        {
            bool result;
            using (HMACSHA512? hmac = new HMACSHA512(user.PasswordSalt))
            {
                var compute = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                result = compute.SequenceEqual(user.PasswordHash);
            }
            return result;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] Register registerModel)
        {
            var user = new User { Username = registerModel.Username, Role = registerModel.Role };
            user.Role ="Admin";
            var userExisted = _userRepository.GetByUserName(user.Username);
            if (userExisted != null)
            {
                return BadRequest("UserName already exist");
            }
            if (registerModel.ConfirmPassword == registerModel.Password)
            {
                using (HMACSHA512? hmac = new HMACSHA512())
                {
                    user.PasswordSalt = hmac.Key;
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerModel.Password));
                }
            }
            else
            {
                return BadRequest("Password dont match");
            }

            if (_userRepository.AddUser(user))
            {
                return Ok("User successfully registered");
            }
            else
            {
                ModelState.AddModelError("", "Error when registering user");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
