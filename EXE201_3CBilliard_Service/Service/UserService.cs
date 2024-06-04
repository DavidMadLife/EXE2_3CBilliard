using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Repository.Tools;
using EXE201_3CBilliard_Service.Exceptions;
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
        private readonly EXE201_3CBilliard_Repository.Tools.Firebase _firebase;


        public UserService(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork, IEmailService emailService, EXE201_3CBilliard_Repository.Tools.Firebase firebase)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _firebase = firebase;
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
                Image = null,
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
            var existingUser = _unitOfWork.UserRepository.Get(filter: v => v.Email == request.Email).FirstOrDefault();
            if (existingUser != null)
            {
                throw new EmailAlreadyExistsException($"Email {request.Email} already exists.");
            }


            var user = _mapper.Map<User>(request);
            user.IdentificationCardNumber = "";
            user.Status = UserStatus.ACTIVE;
            user.Password = HashPassword(request.Password);
            user.CreateAt = DateTime.Now;
            user.ModifineAt = DateTime.Now;
            user.RoleId = 3;
           
            user.Note = "Success";


            if (request.Image != null)
            {
                if (request.Image.Length >= 10 * 1024 * 1024)
                {
                    throw new Exception();
                }
                string imageDownloadUrl = await _firebase.UploadImage(request.Image);
                user.Image = imageDownloadUrl;
            }

            


            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();

            var registerResponse = _mapper.Map<RegisterUserResponse>(user);
            return  registerResponse;
        }

        public async Task<User[]> SearchUser(string? keyword, int pageNumber = 1, int pageSize = 10)
        {
            // Tạo điều kiện lọc dựa trên từ khóa
            Expression<Func<User, bool>> filter = p =>
                string.IsNullOrEmpty(keyword) ||
                p.UserName.Contains(keyword) ||
                p.Email.Contains(keyword);

            // Lấy danh sách người dùng từ repository
            var users = _unitOfWork.UserRepository.Get(
                filter: filter,
                includeProperties: "Role",
                pageIndex: pageNumber,
                pageSize: pageSize
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
        new Claim("userid", info.Id.ToString()),
        new Claim("email", info.Email),
        new Claim("name", info.UserName),
        new Claim("Phone", info.Phone)
    };

            // Add role claim if role information is available
            if (info.Role != null)
            {
                claims.Add(new Claim("role", info.Role.RoleName));
            }
            else
            {
                var role = _unitOfWork.RoleRepository.Get(filter: r => r.Id == info.RoleId).FirstOrDefault();
                if (role != null)
                {
                    claims.Add(new Claim("role", role.RoleName));
                }
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
        public string HashPassword(string password)
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

        
    }
}
