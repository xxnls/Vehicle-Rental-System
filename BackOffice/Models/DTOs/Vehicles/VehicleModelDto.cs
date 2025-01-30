using BackOffice.Models.Vehicles.VehicleBrands.DTOs;

namespace BackOffice.Models.DTOs.Vehicles
{
    public class VehicleModelDto : BaseDtoModel
    {
        public int VehicleModelId { get; set; }

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

        private double? _engineSize;
        public double? EngineSize
        {
            get => _engineSize;
            set
            {
                _engineSize = value;
                OnPropertyChanged();
            }
        }

        private int? _horsePower;
        public int? HorsePower
        {
            get => _horsePower;
            set
            {
                _horsePower = value;
                OnPropertyChanged();
            }
        }

        private string _fuelType = null!;
        public string FuelType
        {
            get => _fuelType;
            set
            {
                _fuelType = value;
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

        // Navigation Properties
        private VehicleBrandDto _vehicleBrand = null!;
        public VehicleBrandDto VehicleBrand
        {
            get => _vehicleBrand;
            set
            {
                _vehicleBrand = value;
                OnPropertyChanged();
            }
        }

    }
}
