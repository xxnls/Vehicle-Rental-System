using API.Interfaces;

namespace API.Models.DTOs.Other
{
    public class RentalPlaceDto : IBaseModel
    {
        public int RentalPlaceId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }

        // Navigation properties
        public AddressDto? Address { get; set; }
        public LocationDto? Location { get; set; }
    }
}
