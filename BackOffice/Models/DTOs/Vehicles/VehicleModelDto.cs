namespace BackOffice.Models.DTOs.Vehicles
{
    public class VehicleModelDto : BaseDtoModel
    {
        public int VehicleModelId { get; set; }

        private int? _vehicleBrandId;
        public int? VehicleBrandId
        {
            get => _vehicleBrandId;
            set
            {
                _vehicleBrandId = value;
                OnPropertyChanged();
            }
        }

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
    }
}
