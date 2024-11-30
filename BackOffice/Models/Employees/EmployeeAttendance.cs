using System;
using System.Collections.Generic;

namespace BackOffice.Models.Employees;

public partial class EmployeeAttendance
{
    public int EmployeeAttendanceId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime CheckInTime { get; set; }

    public DateTime CheckOutTime { get; set; }

    public string Notes { get; set; } = null!;

    public double? TotalTime { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
