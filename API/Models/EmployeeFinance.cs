using System;
using System.Collections.Generic;

namespace API.Models;

public partial class EmployeeFinance
{
    public int EmployeeFinancesId { get; set; }

    public decimal? BaseSalary { get; set; }

    public decimal? HourlyRate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
