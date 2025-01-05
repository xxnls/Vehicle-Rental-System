namespace API.Models.DTOs
{
    public class ChangePasswordRequestDto
    {
        public string Username { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
