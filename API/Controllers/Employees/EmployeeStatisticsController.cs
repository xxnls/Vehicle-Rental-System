using API.Models.DTOs.Employees;
using API.Models.Employees;
using API.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeStatisticsController(EmployeeStatisticsService service)
        : BaseApiController<EmployeeStatistic, EmployeeStatisticsDto, EmployeeStatisticsDto>(service)
    {
        protected override int GetEntityId(EmployeeStatisticsDto entity)
        {
            return entity.EmployeeStatisticsId;
        }
    }
}
