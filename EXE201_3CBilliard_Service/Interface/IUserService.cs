using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IUserService
    {
        Task<string> AuthorizeUser(LoginView loginView);
        Task<User> CreateUserGoogle(GoogleLoginView googleLoginView);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(long id);
        
        /*Task<User[]> SearchUser(SearchUserView searchView);*/
    }
}
