using API.Models;
using API.Models.Customers;
using API.Models.DTOs.Customers;
using API.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerTypesController(CustomerTypesService service) : BaseApiController<CustomerType, CustomerTypeDto, CustomerTypeDto>(service)
    {
        protected override int GetEntityId(CustomerTypeDto entity)
        {
            return entity.CustomerTypeId;
        }
    }
}