namespace API.Models.DTOs.Other
{
    public class UploadLicenseDto
    {
        public byte[] FileContentFront { get; set; } = null!;
        public byte[] FileContentBack { get; set; } = null!;
        public string FileNameFront { get; set; } = null!;
        public string FileNameBack { get; set; } = null!;
        public int CustomerId { get; set; }
        public string LicenseType { get; set; } = null!;
    }
}
