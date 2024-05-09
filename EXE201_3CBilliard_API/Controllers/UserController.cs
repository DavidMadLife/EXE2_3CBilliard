using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginView loginView)
        {
            var token = await _userService.AuthorizeUser(loginView);
            if (token != null)
            {
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginView googleLoginView)
        {
            try
            {
                if(string.IsNullOrEmpty(googleLoginView.UserName) || string.IsNullOrEmpty(googleLoginView.Email))
                {
                    return BadRequest("Invalid info user to login");
                }
                var user = await _userService.GetUserByEmail(googleLoginView.UserName);
                if(user == null)
                {
                    user = await _userService.CreateUserGoogle(googleLoginView);
                }
                else
                {
                    if(user.Status == "Inactive")
                    {
                        return BadRequest("User have been banned");
                    }
                }

                var token = await _userService.AuthorizeUser(new LoginView { Email = googleLoginView.Email, Password = user.Password });
                return Ok(new { Token = token });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(long id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
