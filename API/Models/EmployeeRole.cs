using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Models;

public partial class EmployeeRole : IdentityRole<int>
{
    public int RolePower { get; set; }
    
    public bool ManageVehicles { get; set; }

    public bool ManageEmployees { get; set; }

    public bool ManageRentals { get; set; }

    public bool ManageLeaves { get; set; }

    public bool ManageSchedule { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
