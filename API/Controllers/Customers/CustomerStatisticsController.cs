using API.Models.Customers;
using API.Models.DTOs.Customers;
using API.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerStatisticsController(CustomerStatisticsService service) : BaseApiController<CustomerStatistic, CustomerStatisticsDto, CustomerStatisticsDto>(service)
    {
        protected override int GetEntityId(CustomerStatisticsDto entity)
        {
            return entity.CustomerStatisticsId;
        }
    }
}
