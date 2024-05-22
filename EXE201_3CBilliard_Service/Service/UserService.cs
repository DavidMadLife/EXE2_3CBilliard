using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Service
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Dictionary<string, (string Otp, DateTime Expiry)> _otpStore = new Dictionary<string, (string, DateTime)>();
        private readonly IEmailService _emailService;

        public UserService(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<string> AuthorizeUser(LoginView loginView)
        {
            // Retrieve the user by email first
            var user = _unitOfWork.UserRepository.Get(filter: m => m.Email == loginView.Email && m.Status == UserStatus.ACTIVE).FirstOrDefault();

            if (user != null && VerifyPassword(loginView.Password, user.Password))
            {
                // Generate token
                string token = GenerateToken(user);
                return token;
            }

            return null;
        }


        public async Task<User> CreateUserGoogle(GoogleLoginView googleLoginView)
        {
            User user = new User
            {
                RoleId = 2,
                Email = googleLoginView.Email,
                UserName = googleLoginView.UserName,
                Password = HashPassword("123456"),
                Phone = "",
                IdentificationCardNumber = "",
                Image = "",
                Address = "",
                CreateAt = DateTime.Now,
                ModifineAt = DateTime.Now,
                DoB = new(),
                Note = "",
                Status = UserStatus.ACTIVE
            };
            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();
            return user;

        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = _unitOfWork.UserRepository.Get(filter:(m) => m.Email == email).FirstOrDefault();
            return user;
        }

        public async Task<User?> GetUserById(long id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            return user;
        }

        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
        {
            //Check Useremial exist
            var existingUser = _unitOfWork.UserRepository.Get(filter: v => v.Email == request.Email);
           /* if (existingUser != null)
            {
                return new RegisterUserResponse
                {
                    Id = 0, // Use a sentinel value to indicate failure
                    RoleId = 0,
                    UserName = null,
                    Email = request.Email,
                    Password = null,
                    Phone = null,
                    IdentificationCardNumber = null,
                    Image = null,
                    Address = null,
                    CreateAt = DateTime.MinValue,
                    ModifineAt = DateTime.MinValue,
                    DoB = DateTime.MinValue,
                    Note = "Email already exists.",
                    Status = UserStatus.INACTIVE.ToString() // Use a relevant status or an empty string
                };
            }*/
            var user = _mapper.Map<User>(request);
            user.Status = UserStatus.ACTIVE;
            user.Password = HashPassword(request.Password);
            user.CreateAt = DateTime.Now;
            user.ModifineAt = DateTime.Now;
            /*user.IdentificationCardNumber = HashPassword()*/
            user.Note = "Success";
            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();
            return _mapper.Map<RegisterUserResponse>(user);
        }

        public async Task<User[]> SearchUser(SearchUserView searchView)
        {
            // Tạo điều kiện lọc dựa trên từ khóa
            Expression<Func<User, bool>> filter = p =>
                string.IsNullOrEmpty(searchView.Keyword) ||
                p.UserName.Contains(searchView.Keyword) ||
                p.Address.Contains(searchView.Keyword);

            // Lấy danh sách người dùng từ repository
            var users = _unitOfWork.UserRepository.Get(
                filter: filter,
                includeProperties: "Role", // Đảm bảo Repository hỗ trợ IncludeProperties
                pageIndex: searchView.Page_number, // Sửa lỗi chính tả: Page_number -> PageNumber
                pageSize: searchView.Page_size // Sửa lỗi chính tả: Page_number -> PageSize
            );

            // Trả về danh sách người dùng
            return users.ToArray(); // Sử dụng ToArray() để chuyển đổi IEnumerable sang mảng User[]
        }


        private string GenerateToken(User info)
        {
            List<Claim> claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, info.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString(), ClaimValueTypes.Integer64),
            new Claim("email", info.Email),
            new Claim("name", info.UserName)
           
        };

            // Add role claim if role information is available
            if (info.Role != null)
            {
                var role = _unitOfWork.RoleRepository.Get(filter: r => r.Id == info.RoleId).FirstOrDefault();
                claims.Add(new Claim("role", role.RoleName));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        //Encrypt
        // Method to verify the hashed password
        private bool VerifyPassword(string providedPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }



        // Method to hash the password
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<ChangePasswordResponse> ChangePassword(long id, ChangePasswordRequest changePasswordRequest)
        {
            // Retrieve the user from the repository based on user ID or other identifier
            var user = _unitOfWork.UserRepository.GetById(id);

            // Verify the current password
            if (!VerifyPassword(changePasswordRequest.CurrentPassword, user.Password))
            {
                return new ChangePasswordResponse { Message = "Failed to change password. Current password is incorrect." };
            }

            // Hash the new password
            string hashedNewPassword = HashPassword(changePasswordRequest.NewPassword);

            // Update the user's password
            user.Password = hashedNewPassword;
            user.ModifineAt = DateTime.Now; // Update modification time

            // Save changes to the database
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();

            return new ChangePasswordResponse { NewPassword = hashedNewPassword, Message = "Password changed successfully." };
        }


        //Forgot password
        public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = _unitOfWork.UserRepository.Get(filter: m => m.Email == request.Email).FirstOrDefault();
            if(user == null)
            {
                return new ForgotPasswordResponse { Message = "Email not found" };
            }

            var otp = GenerateOtp();
            _otpStore[user.Email] = (otp, DateTime.Now.AddMinutes(3));
            await _emailService.SendEmailAsync(user.Email, "Your OTP Code", $"Your OTP code is {otp}");

            return new ForgotPasswordResponse { Message = "OTP has been sent to your email." };

        }

        

        //Generate OTP
        private string GenerateOtp()
        {
            // Simple OTP generation logic. You can replace this with a more secure implementation.
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }


        //Check OTP
        public async Task<ValidateOtpResponse> ValidateOtp(ValidateOtpRequest request)
        {
            if(_otpStore.TryGetValue(request.Email, out var otpInfor))
            {
                if(otpInfor.Otp == request.Otp && otpInfor.Expiry > DateTime.Now)
                {
                    return new ValidateOtpResponse { Message = "OTP is valid", IsValid = true };
                }
            }

            return new ValidateOtpResponse { Message = "Invalid or expired OTP", IsValid = false };
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var valtedateOtpResponse = await ValidateOtp(new ValidateOtpRequest { Email = request.Email,Otp = request.Otp });
            if(!valtedateOtpResponse.IsValid)
            {
                return new ResetPasswordResponse { Message = "Invalid or expired OTP" };
            }

            var user = _unitOfWork.UserRepository.Get(filter: m => m.Email == request.Email).FirstOrDefault();
            if(user == null)
            {
                return new ResetPasswordResponse { Message = "Invalid email" };
            }
            user.Password = HashPassword(request.NewPassword);
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();


            //Remove OTP after password reset
            _otpStore.Remove(request.Email);
            return new ResetPasswordResponse { Message = "Password has been reset successfully" };

        }
    }
}
