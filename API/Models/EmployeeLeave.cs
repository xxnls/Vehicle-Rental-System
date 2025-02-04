using API.Models.Employees;
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class EmployeeLeave
{
    public int EmployeeLeaveId { get; set; }

    public int EmployeeId { get; set; }

    public int EmployeeLeaveTypeId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public int ApprovedById { get; set; }

    public DateTime ApprovalDate { get; set; }

    public string? Reason { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Employee ApprovedBy { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual EmployeeLeaveType EmployeeLeaveType { get; set; } = null!;
}
