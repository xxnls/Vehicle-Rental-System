using API.Models.DTOs.Other;
using API.Models.Other;
using API.Services.Other;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Other
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController(CountriesService service) : BaseApiController<Country, CountryDto, CountryDto>(service)
    {
        protected override int GetEntityId(CountryDto entity)
        {
            return entity.CountryId;
        }
    }
}
