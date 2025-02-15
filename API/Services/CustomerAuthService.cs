using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using API.Models.DTOs;
using API.Models.Customers;
using API.Services.Customers;

namespace API.Services
{
    public class CustomerAuthService
    {
        private readonly UserManager<Customer> _userManager;
        private readonly IConfiguration _configuration;
        private readonly PasswordValidator<Customer> _passwordValidator;
        private readonly CustomersService _customersService;

        public CustomerAuthService(UserManager<Customer> userManager, IConfiguration configuration, CustomersService customers)
        {
            _userManager = userManager;
            _configuration = configuration;
            _passwordValidator = new PasswordValidator<Customer>();
            _customersService = customers;
        }

        /// <summary>
        /// Authenticates a customer by their email and password, and generates a JWT token if the credentials are valid.
        /// </summary>
        /// <param name="username">The username of the customer trying to log in.</param>
        /// <param name="password">The password provided by the customer for authentication.</param>
        /// <returns>
        /// A <see cref="LoginResponseDto"/> containing a JWT token and the customer's ID if the login is successful; 
        /// otherwise, returns <c>null</c> if the username or password is incorrect.
        /// </returns>
        public async Task<CustomerLoginResponseDto?> LoginAsync(string email, string password)
        {
            var customer = await _userManager.FindByEmailAsync(email);
            if (customer == null || !await _userManager.CheckPasswordAsync(customer, password))
            {
                return null; // Invalid username or password
            }

            // Generate JWT
            var token = GenerateJwtToken(customer);

            var customerDto = await _customersService.GetByIdAsync(customer.Id);

            // Return the token and customer ID
            return new CustomerLoginResponseDto
            {
                Token = token,
                Customer = customerDto
            };
        }

        /// <summary>
        /// Changes the customer's password by validating the new password and updating it in the database.
        /// </summary>
        /// <param name="username">The username of the customer whose password is being changed.</param>
        /// <param name="currentPassword">The current password of the customer.</param>
        /// <param name="newPassword">The new password that the customer wants to set.</param>
        /// <returns>A result indicating whether the password change was successful, along with any validation errors.</returns>
        public async Task<ChangePasswordResult> ChangePasswordAsync(string username, string currentPassword, string newPassword)
        {
            var result = new ChangePasswordResult();

            var customer = await _userManager.FindByNameAsync(username);
            if (customer == null)
            {
                result.Message = "Customer not found";
                return result;
            }

            // Validate the new password
            var passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, customer, newPassword);
            if (!passwordValidationResult.Succeeded)
            {
                result.Message = "Password does not meet the required rules.";
                result.Errors = passwordValidationResult.Errors.Select(e => e.Description).ToList();
                return result;
            }

            // Attempt to change the password
            var passwordChangeResult = await _userManager.ChangePasswordAsync(customer, currentPassword, newPassword);
            if (!passwordChangeResult.Succeeded)
            {
                result.Message = "Failed to change password. Please check your current password and try again.";
                return result;
            }

            result.Success = true;
            result.Message = "Password changed successfully";
            return result;
        }

        private string GenerateJwtToken(Customer customer)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customer.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, customer.UserName),
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString())
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