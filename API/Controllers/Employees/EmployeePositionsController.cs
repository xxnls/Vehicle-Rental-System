using API.Models;
using API.Models.DTOs.Employees;
using API.Models.Employees;
using API.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeePositionsController(EmployeePositionsService service) : BaseApiController<EmployeePosition, EmployeePositionDto, EmployeePositionDto>(service)
    {
        protected override int GetEntityId(EmployeePositionDto entity)
        {
            return entity.EmployeePositionId;
        }
    }
}