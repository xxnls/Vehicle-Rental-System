using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Employees;

namespace BackOffice.Models.DTOs.Rentals
{
    public class PostRentalReportDto : BaseDtoModel
    {
        public int PostRentalReportId { get; set; }

        public int InspectorEmployeeId { get; set; }

        private bool _isCustomerLate;
        public bool IsCustomerLate
        {
            get => _isCustomerLate;
            set
            {
                if (_isCustomerLate != value)
                {
                    _isCustomerLate = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isCarDamaged;
        public bool IsCarDamaged
        {
            get => _isCarDamaged;
            set
            {
                if (_isCarDamaged != value)
                {
                    _isCarDamaged = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isCarRefueled;
        public bool IsCarRefueled
        {
            get => _isCarRefueled;
            set
            {
                if (_isCarRefueled != value)
                {
                    _isCarRefueled = value;
                    OnPropertyChanged();
                }
            }
        }

        private EmployeeDto _inspectorEmployee = null!;
        public EmployeeDto InspectorEmployee
        {
            get => _inspectorEmployee;
            set
            {
                if (_inspectorEmployee != value)
                {
                    _inspectorEmployee = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
