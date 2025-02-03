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

        private string _firstLine = null!;
        public string FirstLine
        {
            get => _firstLine;
            set
            {
                _firstLine = value;
                OnPropertyChanged();
            }
        }

        private string? _secondLine;
        public string? SecondLine
        {
            get => _secondLine;
            set
            {
                _secondLine = value;
                OnPropertyChanged();
            }
        }

        private string _zipCode = null!;
        public string ZipCode
        {
            get => _zipCode;
            set
            {
                _zipCode = value;
                OnPropertyChanged();
            }
        }

        private string _city = null!;
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

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
