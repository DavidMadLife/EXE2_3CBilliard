using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Service
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync()
        {
            var roles = _unitOfWork.RoleRepository.Get();
            return _mapper.Map<IEnumerable<RoleResponse>>(roles);
        }

        public async Task<RoleResponse> GetRoleByIdAsync(long id)
        {
            var role = _unitOfWork.RoleRepository.GetById(id);
            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<RoleResponse> CreateRoleAsync(RoleRequest request)
        {
            var role = _mapper.Map<Role>(request);
            _unitOfWork.RoleRepository.Insert(role);
            _unitOfWork.Save();
            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<RoleResponse> UpdateRoleAsync(long id, RoleRequest request)
        {
            var role = _unitOfWork.RoleRepository.GetById(id);
            if (role == null)
                throw new Exception($"Role with id {id} not found.");

            _mapper.Map(request, role);
            _unitOfWork.RoleRepository.Update(role);
            _unitOfWork.Save();
            return _mapper.Map<RoleResponse>(role);
        }

        public async Task<bool> DeleteRoleAsync(long id)
        {
            var role = _unitOfWork.RoleRepository.GetById(id);
            if (role == null)
                throw new Exception($"Role with id {id} not found.");

            _unitOfWork.RoleRepository.Delete(role);
            _unitOfWork.Save();
            return true;
        }
    }
}
