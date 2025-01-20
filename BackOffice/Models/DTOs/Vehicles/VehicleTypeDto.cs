namespace BackOffice.Models.DTOs.Vehicles
{
    public class VehicleTypeDto : BaseDtoModel
    {
        private string _name = null!;
        private string? _description;
        private decimal _baseDailyRate;
        private decimal _baseWeeklyRate;
        private decimal _baseDeposit;
        private string _requiredLicenseType = null!;

        public int VehicleTypeId { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal BaseDailyRate
        {
            get => _baseDailyRate;
            set
            {
                if (_baseDailyRate != value)
                {
                    _baseDailyRate = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal BaseWeeklyRate
        {
            get => _baseWeeklyRate;
            set
            {
                if (_baseWeeklyRate != value)
                {
                    _baseWeeklyRate = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal BaseDeposit
        {
            get => _baseDeposit;
            set
            {
                if (_baseDeposit != value)
                {
                    _baseDeposit = value;
                    OnPropertyChanged();
                }
            }
        }

        public string RequiredLicenseType
        {
            get => _requiredLicenseType;
            set
            {
                if (_requiredLicenseType != value)
                {
                    _requiredLicenseType = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
