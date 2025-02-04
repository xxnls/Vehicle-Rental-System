using API.Models.DTOs.Employees;
using API.Models.Employees;
using API.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeFinancesController(EmployeeFinancesService service)
        : BaseApiController<EmployeeFinance, EmployeeFinanceDto, EmployeeFinanceDto>(service)
    {
        protected override int GetEntityId(EmployeeFinanceDto entity)
        {
            return entity.EmployeeFinancesId;
        }
    }
}
