using API.Models;
using API.Models.DTOs.Employees;
using API.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeLeaveTypesController(EmployeeLeaveTypesService service) : BaseApiController<EmployeeLeaveType, EmployeeLeaveTypeDto, EmployeeLeaveTypeDto>(service)
    {
        protected override int GetEntityId(EmployeeLeaveTypeDto entity)
        {
            return entity.EmployeeLeaveTypeId;
        }
    }
}
