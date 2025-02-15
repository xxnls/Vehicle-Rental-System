using API.Models.DTOs.Customers;

namespace API.Models.DTOs
{
    public class CustomerLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public CustomerDto Customer { get; set; }
    }
}
