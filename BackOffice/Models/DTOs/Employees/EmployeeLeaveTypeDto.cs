using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Employees
{
    public class EmployeeLeaveTypeDto : BaseDtoModel
    {
        public int EmployeeLeaveTypeId { get; set; }

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

        private int _defaultDays;
        public int DefaultDays
        {
            get => _defaultDays;
            set
            {
                _defaultDays = value;
                OnPropertyChanged();
            }
        }
    }
}
