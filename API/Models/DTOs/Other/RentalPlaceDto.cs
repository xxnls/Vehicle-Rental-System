using API.Interfaces;

namespace API.Models.DTOs.Other
{
    public class RentalPlaceDto : IBaseModel
    {
        public int RentalPlaceId { get; set; }

        public int LocationId { get; set; }

        public double? Gpslatitude { get; set; }

        public double? Gpslongitude { get; set; }

        public int AddressId { get; set; }

        public string? CountryName { get; set; } = null!;

        public string? City { get; set; } = null!;

        public string? FirstLine { get; set; } = null!;

        public string? SecondLine { get; set; } = null!;

        public string? ZipCode { get; set; } = null!;

        public short CountryId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }


        public AddressDto? Address { get; set; }
    }
}
