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
                return BadRequest(new { Message = "Invalid login request" });

            var loginResponse = await _authService.LoginAsync(request.Username, request.Password);

            if (loginResponse == null)
                return Unauthorized(new { Message = "Invalid username or password" });

            return Ok(loginResponse);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid request" });

            var passwordChangeResult = await _authService.ChangePasswordAsync(
                request.Username,
                request.CurrentPassword,
                request.NewPassword
            );

            // Check if the password change was successful
            if (passwordChangeResult.Success)
            {
                return Ok(new { Message = passwordChangeResult.Message });
            }

            // If not successful, return the message and errors
            return BadRequest(new
            {
                Message = passwordChangeResult.Message,
                Errors = passwordChangeResult.Errors
            });
        }

    }
}