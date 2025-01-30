using API.Models.DTOs.Employees;
using API.Models.Employees;
using API.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeShiftTypesController(EmployeeShiftTypesService service) : BaseApiController<EmployeeShiftType, EmployeeShiftTypeDto, EmployeeShiftTypeDto>(service)
    {
        protected override int GetEntityId(EmployeeShiftTypeDto entity)
        {
            return entity.EmployeeShiftTypeId;
        }
    }
}
