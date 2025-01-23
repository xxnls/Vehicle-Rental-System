using API.Interfaces;

namespace API.Models.DTOs.Other
{
    public class AddressDto : IBaseModel
    {
        public int AddressId { get; set; }

        public short CountryId { get; set; }

        public string? CountryName { get; set; } = null!;

        public string FirstLine { get; set; } = null!;

        public string? SecondLine { get; set; }

        public string ZipCode { get; set; } = null!;

        public string City { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }

        // Navigation properties
        public CountryDto? Country { get; set; }
    }
}
