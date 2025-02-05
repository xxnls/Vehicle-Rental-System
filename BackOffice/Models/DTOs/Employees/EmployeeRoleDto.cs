namespace BackOffice.Models.DTOs.Employees
{
    public class EmployeeRoleDto : BaseDtoModel
    {
        public int Id { get; set; } // Role ID (inherited from IdentityRole)

        private string _name = null!;
        private int _rolePower;
        private bool _manageVehicles;
        private bool _manageEmployees;
        private bool _manageRentals;
        private bool _manageLeaves;
        private bool _manageSchedule;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public int RolePower
        {
            get => _rolePower;
            set
            {
                if (_rolePower != value)
                {
                    _rolePower = value;
                    OnPropertyChanged(nameof(RolePower));
                }
            }
        }

        public bool ManageVehicles
        {
            get => _manageVehicles;
            set
            {
                if (_manageVehicles != value)
                {
                    _manageVehicles = value;
                    OnPropertyChanged(nameof(ManageVehicles));
                }
            }
        }

        public bool ManageEmployees
        {
            get => _manageEmployees;
            set
            {
                if (_manageEmployees != value)
                {
                    _manageEmployees = value;
                    OnPropertyChanged(nameof(ManageEmployees));
                }
            }
        }

        public bool ManageRentals
        {
            get => _manageRentals;
            set
            {
                if (_manageRentals != value)
                {
                    _manageRentals = value;
                    OnPropertyChanged(nameof(ManageRentals));
                }
            }
        }

        public bool ManageLeaves
        {
            get => _manageLeaves;
            set
            {
                if (_manageLeaves != value)
                {
                    _manageLeaves = value;
                    OnPropertyChanged(nameof(ManageLeaves));
                }
            }
        }

        public bool ManageSchedule
        {
            get => _manageSchedule;
            set
            {
                if (_manageSchedule != value)
                {
                    _manageSchedule = value;
                    OnPropertyChanged(nameof(ManageSchedule));
                }
            }
        }
        public DateTime? ModifiedDate { get; set; }
    }
}
