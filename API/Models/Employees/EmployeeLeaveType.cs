using System;
using System.Collections.Generic;

namespace API.Models.Employees;

public partial class EmployeeLeaveType
{
    public int EmployeeLeaveTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int DefaultDays { get; set; }

    public virtual ICollection<EmployeeLeave> EmployeeLeaves { get; set; } = new List<EmployeeLeave>();
}
