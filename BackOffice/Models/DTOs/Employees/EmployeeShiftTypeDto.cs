using BackOffice.Models;

namespace BackOffice.Models.DTOs.Employees
{
    public class EmployeeShiftTypeDto : BaseDtoModel
    {
        public int EmployeeShiftTypeId { get; set; }

        private string? _name;
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private DateTime _timeStart;
        public DateTime TimeStart
        {
            get => _timeStart;
            set
            {
                _timeStart = value;
                OnPropertyChanged();
            }
        }

        private DateTime _timeEnd;
        public DateTime TimeEnd
        {
            get => _timeEnd;
            set
            {
                _timeEnd = value;
                OnPropertyChanged();
            }
        }
    }
}
