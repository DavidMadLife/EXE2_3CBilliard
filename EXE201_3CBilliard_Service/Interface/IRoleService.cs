using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync();
        Task<RoleResponse> GetRoleByIdAsync(long id);
        Task<RoleResponse> CreateRoleAsync(RoleRequest request);
        Task<RoleResponse> UpdateRoleAsync(long id, RoleRequest request);
        Task<bool> DeleteRoleAsync(long id);
    }
}
