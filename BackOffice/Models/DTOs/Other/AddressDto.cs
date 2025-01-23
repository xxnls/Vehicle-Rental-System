using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Other
{
    public class AddressDto : BaseDtoModel
    {
        public int AddressId { get; set; }

        public string FirstLine { get; set; } = null!;

        public string? SecondLine { get; set; }

        public string ZipCode { get; set; } = null!;

        public string City { get; set; } = null!;

        // Navigation properties
        private CountryDto? _country;
        public CountryDto? Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }
    }
}
