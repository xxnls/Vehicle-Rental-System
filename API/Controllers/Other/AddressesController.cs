using API.Models;
using API.Models.DTOs.Other;
using API.Services.Other;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Other
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController(AddressesService service) : BaseApiController<Address, AddressDto, AddressDto>(service)
    {
        protected override int GetEntityId(AddressDto entity)
        {
            return entity.AddressId;
        }
    }
}
