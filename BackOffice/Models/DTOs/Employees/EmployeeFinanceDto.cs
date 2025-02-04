using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Employees
{
    public class EmployeeFinanceDto : BaseDtoModel
    {
        private int _employeeFinancesId;
        private decimal? _baseSalary;
        private decimal? _hourlyRate;
        private DateTime? _modifiedDate;

        public int EmployeeFinancesId
        {
            get => _employeeFinancesId;
            set { _employeeFinancesId = value; OnPropertyChanged(); }
        }

        public decimal? BaseSalary
        {
            get => _baseSalary;
            set { _baseSalary = value; OnPropertyChanged(); }
        }

        public decimal? HourlyRate
        {
            get => _hourlyRate;
            set { _hourlyRate = value; OnPropertyChanged(); }
        }

        public new DateTime? ModifiedDate
        {
            get => _modifiedDate;
            set { _modifiedDate = value; OnPropertyChanged(); }
        }
    }
}
