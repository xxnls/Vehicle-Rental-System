using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.DTOs.Employees
{
    public class EmployeeStatisticsDto : BaseDtoModel
    {
        private int _employeeStatisticsId;
        private int _totalWorkDays;
        private int _lateArrivals;
        private int _earlyDepartures;
        private double _overtimeHours;
        private int _sickLeavesTaken;
        private int _vacationDaysTaken;
        private int _unpaidLeavesTaken;
        private int? _totalRentalsApproved;

        public int EmployeeStatisticsId
        {
            get => _employeeStatisticsId;
            set { _employeeStatisticsId = value; OnPropertyChanged(); }
        }

        public int TotalWorkDays
        {
            get => _totalWorkDays;
            set { _totalWorkDays = value; OnPropertyChanged(); }
        }

        public int LateArrivals
        {
            get => _lateArrivals;
            set { _lateArrivals = value; OnPropertyChanged(); }
        }

        public int EarlyDepartures
        {
            get => _earlyDepartures;
            set { _earlyDepartures = value; OnPropertyChanged(); }
        }

        public double OvertimeHours
        {
            get => _overtimeHours;
            set { _overtimeHours = value; OnPropertyChanged(); }
        }

        public int SickLeavesTaken
        {
            get => _sickLeavesTaken;
            set { _sickLeavesTaken = value; OnPropertyChanged(); }
        }

        public int VacationDaysTaken
        {
            get => _vacationDaysTaken;
            set { _vacationDaysTaken = value; OnPropertyChanged(); }
        }

        public int UnpaidLeavesTaken
        {
            get => _unpaidLeavesTaken;
            set { _unpaidLeavesTaken = value; OnPropertyChanged(); }
        }

        public int? TotalRentalsApproved
        {
            get => _totalRentalsApproved;
            set { _totalRentalsApproved = value; OnPropertyChanged(); }
        }
    }
}
