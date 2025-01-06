using System;
using System.Collections.Generic;
using BackOffice.Models.Employees;

namespace BackOffice.Models.Employees;

public partial class EmployeeFinances
{
    public int EmployeeFinancesId { get; set; }

    public int ApprovedById { get; set; }

    public decimal? BaseSalary { get; set; }

    public decimal? HourlyRate { get; set; }

    public decimal Bonuses { get; set; }

    public decimal Allowances { get; set; }

    public decimal Deductions { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee ApprovedBy { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
