using API.Models;
using API.Models.DTOs.Employees;
using API.Services.Employees;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeRolesController(EmployeeRolesService service) : BaseApiController<EmployeeRole, EmployeeRoleDto, EmployeeRoleDto>(service)
    {
        protected override int GetEntityId(EmployeeRoleDto entity)
        {
            return entity.Id;
        }

        [HttpGet("user/{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles = await service.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        [HttpPost("user/{userId}/roles/{roleName}")]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var result = await service.AssignRoleAsync(userId, roleName);
            if (!result)
                return BadRequest("Failed to assign role to user.");
            return Ok();
        }

        [HttpDelete("user/{userId}/roles/{roleName}")]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var result = await service.RemoveRoleAsync(userId, roleName);
            if (!result)
                return BadRequest("Failed to remove role from user.");
            return Ok();
        }

        [HttpGet("user/{userId}/has-permission/{permission}")]
        public async Task<IActionResult> CheckUserPermission(string userId, string permission)
        {
            var result = await service.HasPermissionAsync(userId, permission);
            return Ok(result);
        }
    }
}
