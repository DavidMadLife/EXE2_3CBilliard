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

        public UserService(IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> AuthorizeUser(LoginView loginView)
        {
            var member = _unitOfWork.UserRepository.Get(filter: m => m.Email == loginView.Email && m.Password == loginView.Password).FirstOrDefault();
            if (member != null)
            {
               string token = GenerateToken(member);
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
                Password = "123456",
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
            var user = _mapper.Map<User>(request);
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
            new Claim(ClaimTypes.Email, info.Email),
        };

            // Add role claim if role information is available
            if (info.Role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, info.Role.RoleName));
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


    }
}
