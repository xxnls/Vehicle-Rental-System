using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeAuthController : ControllerBase
    {
        private readonly EmployeeAuthService _authService;

        public EmployeeAuthController(EmployeeAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login request");

            var token = await _authService.LoginAsync(request.Username, request.Password);

            if (token == null)
                return Unauthorized("Invalid username or password");

            return Ok(new { Token = token });
        }
    }
}