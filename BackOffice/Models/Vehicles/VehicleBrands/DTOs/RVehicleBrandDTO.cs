using BackOffice.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BackOffice.Models.Vehicles.VehicleBrands.DTOs
{
    public class RVehicleBrandDTO : BaseDtoModel
    {
        public int VehicleBrandId { get; set; }

        private string _name = null!;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string? _website;
        public string? Website
        {
            get => _website;
            set
            {
                _website = value;
                OnPropertyChanged();
            }
        }

        private string? _logoUrl;
        public string? LogoUrl
        {
            get => _logoUrl;
            set
            {
                _logoUrl = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
