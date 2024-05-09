﻿using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleResponse>> GetById(long id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<RoleResponse>> Create([FromBody] RoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRole = await _roleService.CreateRoleAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdRole.Id }, createdRole);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoleResponse>> Update(long id, [FromBody] RoleRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedRole = await _roleService.UpdateRoleAsync(id, request);
                return Ok(updatedRole);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(id);
                if (result)
                    return NoContent();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
