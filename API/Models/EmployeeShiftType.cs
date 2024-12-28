using System;
using System.Collections.Generic;

namespace API.Models;

public partial class EmployeeShiftType
{
    public int EmployeeShiftTypeId { get; set; }

    public string? Name { get; set; }

    public DateTime TimeStart { get; set; }

    public DateTime TimeEnd { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; } = new List<EmployeeSchedule>();
}
