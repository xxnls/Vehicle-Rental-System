using System.ComponentModel.DataAnnotations;

namespace BackOffice.Models.Vehicles.VehicleBrands.DTOs
{
    public class CUVehicleBrandDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 25 characters.")]
        public string Name { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string? Description { get; set; }

        [StringLength(200, ErrorMessage = "Website URL cannot exceed 100 characters.")]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string? Website { get; set; }

        [StringLength(200, ErrorMessage = "Logo URL cannot exceed 100 characters.")]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string? LogoUrl { get; set; }
    }
}
