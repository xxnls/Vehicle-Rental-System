using API.Models.Other;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.Other
{
    [ApiController]
    [Route("api/settings")]
    public class SettingsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SettingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("currency")]
        public IActionResult GetCurrencySettings()
        {
            var currencySettings = _configuration.GetSection("Currency").Get<CurrencySettings>();
            return Ok(currencySettings);
        }
    }
}
