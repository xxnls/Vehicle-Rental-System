using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Other
{
    public class RentalPlaceDto : BaseDtoModel
    {
        public int RentalPlaceId { get; set; }

        // Navigation properties
        private AddressDto? _address = new();
        public AddressDto? Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private LocationDto? _location = new();
        public LocationDto? Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }
    }
}
