using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
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
                Status = "Active"
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

        /*public Task<User[]> SearchUser(SearchUserView searchView)
        {
            Expression<Func<User, bool>> filter = p =>
            (string.IsNullOrEmpty(searchView.Keyword) || p.UserName.Contains(searchView.Keyword) || p.Address.Contains(searchView.Keyword));
            var user = _unitOfWork.UserRepository.Get(
                filter: filter,
                includeProperties: "Role",
                pageIndex: searchView.Page_number,
                pageSize: searchView.Page_number
                
            );
            if(user == null)
            {
                return 
            }
        }*/

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
