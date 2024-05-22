using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly OtpManager _otpManager;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IUnitOfWork unitOfWork, IEmailService emailService, OtpManager otpManager, IUserService userService)
        {
            _emailService = emailService;
            _otpManager = otpManager;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            _otpManager.RemoveOtp(request.Email); // Remove any existing OTP
            await _emailService.SendOtpEmailAsync(request.Email);
            return Ok(new { message = "OTP has been sent to your email." });
        }


        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOtp([FromBody] ValidateOtpRequest request)
        {
            var isValid = _otpManager.ValidateOtp(request.Email, request.Otp);
            if (isValid)
            {
                var user = await _userService.GetUserByEmail(request.Email);
                if (user != null)
                {
                    user.Password = _userService.HashPassword(request.Password); // Set new password directly
                    _unitOfWork.Save(); // Save changes to the database

                    _otpManager.RemoveOtp(request.Email); // Remove OTP after password change

                    return Ok(new { message = "OTP is valid. Password has been changed." });
                }
                else
                {
                    return BadRequest(new { message = "User not found." });
                }
            }
            else
            {
                return BadRequest(new { message = "Invalid or expired OTP." });
            }
        }

    }
}
