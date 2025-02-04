using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using API.Models.DTOs;
using NuGet.Protocol.Plugins;
using API.Models.Employees;

namespace API.Services
{
    public class EmployeeAuthService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IConfiguration _configuration;
        private readonly PasswordValidator<Employee> _passwordValidator;

        public EmployeeAuthService(UserManager<Employee> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _passwordValidator = new PasswordValidator<Employee>();
        }

        /// <summary>
        /// Authenticates a user by their username and password, and generates a JWT token if the credentials are valid.
        /// </summary>
        /// <param name="username">The username of the user trying to log in.</param>
        /// <param name="password">The password provided by the user for authentication.</param>
        /// <returns>
        /// A <see cref="LoginResponseDto"/> containing a JWT token and the user's ID if the login is successful; 
        /// otherwise, returns <c>null</c> if the username or password is incorrect.
        /// </returns>
        public async Task<LoginResponseDto?> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return null; // Invalid username or password
            }

            // Generate JWT
            var token = GenerateJwtToken(user);

            // Return the token and user ID
            return new LoginResponseDto
            {
                Token = token,
                UserId = user.Id
            };
        }

        /// <summary>
        /// Changes the user's password by validating the new password and updating it in the database.
        /// </summary>
        /// <param name="username">The username of the employee whose password is being changed.</param>
        /// <param name="currentPassword">The current password of the user.</param>
        /// <param name="newPassword">The new password that the user wants to set.</param>
        /// <returns>A result indicating whether the password change was successful, along with any validation errors.</returns>
        public async Task<ChangePasswordResult> ChangePasswordAsync(string username, string currentPassword, string newPassword)
        {
            var result = new ChangePasswordResult();

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                result.Message = "User not found";
                return result;
            }

            // Validate the new password
            var passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, user, newPassword);
            if (!passwordValidationResult.Succeeded)
            {
                result.Message = "Password does not meet the required rules.";
                result.Errors = passwordValidationResult.Errors.Select(e => e.Description).ToList();
                return result;
            }

            // Attempt to change the password
            var passwordChangeResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!passwordChangeResult.Succeeded)
            {
                result.Message = "Failed to change password. Please check your current password and try again.";
                return result;
            }

            result.Success = true;
            result.Message = "Password changed successfully";
            return result;
        }

        private string GenerateJwtToken(Employee user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
