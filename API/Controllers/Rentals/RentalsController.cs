using API.Models.DTOs.Rentals;
using API.Models.Rentals;
using API.Services.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Rentals
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController(RentalsService service) : BaseApiController<Rental, RentalDto, RentalDto>(service)
    {
        protected override int GetEntityId(RentalDto entity)
        {
            return entity.RentalId;
        }
    }
}
