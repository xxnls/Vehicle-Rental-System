using System;
using System.Collections.Generic;

namespace API.Models.Employees;

public partial class EmployeePosition
{
    public int EmployeePositionId { get; set; }

    public string Title { get; set; } = null!;

    public decimal? DefaultBaseSalary { get; set; }

    public decimal? DefaultHourlyRate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
