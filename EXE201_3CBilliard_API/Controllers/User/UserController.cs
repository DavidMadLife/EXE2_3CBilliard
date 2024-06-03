using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Service.Exceptions;
using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers
{
    [Route("api/v1.0/users")]
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
                if (string.IsNullOrEmpty(googleLoginView.UserName) || string.IsNullOrEmpty(googleLoginView.Email))
                {
                    return BadRequest("Invalid info user to login");
                }
                var user = await _userService.GetUserByEmail(googleLoginView.UserName);
                if (user == null)
                {
                    user = await _userService.CreateUserGoogle(googleLoginView);
                }
                else
                {
                    if (user.Status == UserStatus.INACTIVE)
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
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetUserById(long id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string? keyword, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var users = await _userService.SearchUser(keyword, pageNumber, pageSize);
            return Ok(users);
        }


        [HttpPost("register")]
        public async Task<ActionResult<RegisterUserResponse>> RegisterUser([FromBody] RegisterUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userService.RegisterUser(request);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (EmailAlreadyExistsException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost("{id:long}/change-password")]
        public async Task<IActionResult> ChangePassword(long id,[FromBody]ChangePasswordRequest changePasswordRequest)
        {
            var response = await _userService.ChangePassword(id, changePasswordRequest);

            if (!string.IsNullOrEmpty(response.NewPassword))
            {
                // Password changed successfully
                return Ok(response);
            }
            else
            {
                // Failed to change password
                return BadRequest(response);
            }
        }

       

    }
}
