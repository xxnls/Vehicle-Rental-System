using System;
using System.Collections.Generic;

namespace API.Models.Employees;

public partial class EmployeeStatistics
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

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
