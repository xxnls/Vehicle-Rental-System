namespace API.Models.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
