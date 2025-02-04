namespace API.Models.DTOs.Employees
{
    public class EmployeeStatisticsDto
    {
        public int EmployeeStatisticsId { get; set; }

        public int TotalWorkDays { get; set; }

        public int LateArrivals { get; set; }

        public int EarlyDepartures { get; set; }

        public double OvertimeHours { get; set; }

        public int SickLeavesTaken { get; set; }

        public int VacationDaysTaken { get; set; }

        public int UnpaidLeavesTaken { get; set; }

        public int? TotalRentalsApproved { get; set; }
    }
}
