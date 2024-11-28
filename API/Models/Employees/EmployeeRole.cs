using System;
using System.Collections.Generic;

namespace API.Models.Employees;

public partial class EmployeeRole
{
    public int EmployeeRoleId { get; set; }

    public int RolePower { get; set; }

    public bool ManageVehicles { get; set; }

    public bool ManageEmployees { get; set; }

    public bool ManageRentals { get; set; }

    public bool ManageLeaves { get; set; }

    public bool ManageSchedule { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
