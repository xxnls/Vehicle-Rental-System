using API.Models.Other;
using System;
using System.Collections.Generic;
using API.Interfaces;

namespace API.Models.Employees;

public partial class EmployeeSchedule : IBaseModel
{
    public int EmployeeScheduleId { get; set; }

    public int EmployeeShiftTypeId { get; set; }

    public int EmployeeId { get; set; }

    public int? PlaceOfWorkId { get; set; }

    public DateTime Date { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int CreatedByEmployeeId { get; set; }

    public int? ModifiedByEmployeeId { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee CreatedByEmployee { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual EmployeeShiftType EmployeeShiftType { get; set; } = null!;

    public virtual Employee? ModifiedByEmployee { get; set; }

    public virtual RentalPlace? PlaceOfWork { get; set; }
}
