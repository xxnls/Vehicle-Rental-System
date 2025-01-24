using BackOffice.Models.Vehicles.VehicleBrands.DTOs;

namespace BackOffice.Models.DTOs.Vehicles
{
    public class VehicleModelDto : BaseDtoModel
    {
        public int VehicleModelId { get; set; }

        public string Name { get; set; } = null!;

        public double? EngineSize { get; set; }

        public int? HorsePower { get; set; }

        public string FuelType { get; set; } = null!;

        public string? Description { get; set; }

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
