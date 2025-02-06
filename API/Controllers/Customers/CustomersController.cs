using API.Models.Customers;
using API.Models.DTOs.Customers;
using API.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(CustomersService service) : BaseApiController<Customer, CustomerDto, CustomerDto>(service)
    {
        protected override int GetEntityId(CustomerDto entity)
        {
            return entity.Id;
        }
    }
}
