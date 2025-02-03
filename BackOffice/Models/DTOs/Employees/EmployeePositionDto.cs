using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Employees
{
    public class EmployeePositionDto : BaseDtoModel
    {
        public int EmployeePositionId { get; set; }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        private decimal? _defaultBaseSalary;
        public decimal? DefaultBaseSalary
        {
            get => _defaultBaseSalary;
            set
            {
                if (_defaultBaseSalary == value) return;
                _defaultBaseSalary = value;
                OnPropertyChanged();
            }
        }

        private decimal? _defaultHourlyRate;
        public decimal? DefaultHourlyRate
        {
            get => _defaultHourlyRate;
            set
            {
                if (_defaultHourlyRate == value) return;
                _defaultHourlyRate = value;
                OnPropertyChanged();
            }
        }

    }
}
