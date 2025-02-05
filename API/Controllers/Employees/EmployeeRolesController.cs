using API.Models;
using API.Models.DTOs.Employees;
using API.Services.Employees;
using Microsoft.AspNetCore.Mvc;

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
    }
}
