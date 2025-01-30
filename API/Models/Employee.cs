using API.Models.Employees;
using API.Models.Other;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Employee : IdentityUser<int>
{
    public int EmployeeStatisticsId { get; set; }

    public int EmployeeFinancesId { get; set; }

    public int RentalPlaceId { get; set; }

    public int AddressId { get; set; }

    public int EmployeePositionId { get; set; }

    public int? SupervisorId { get; set; }

    public string? Status { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public DateTime HireDate { get; set; }

    public DateTime? TerminationDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Document> DocumentCreatedByEmployees { get; set; } = new List<Document>();

    public virtual ICollection<Document> DocumentEmployees { get; set; } = new List<Document>();

    public virtual ICollection<Document> DocumentModifiedByEmployees { get; set; } = new List<Document>();

    public virtual ICollection<EmployeeAttendance> EmployeeAttendances { get; set; } = new List<EmployeeAttendance>();

    public virtual EmployeeFinance EmployeeFinances { get; set; } = null!;

    public virtual ICollection<EmployeeFinance> EmployeeFinancesNavigation { get; set; } = new List<EmployeeFinance>();

    public virtual ICollection<EmployeeLeave> EmployeeLeaveApprovedBies { get; set; } = new List<EmployeeLeave>();

    public virtual ICollection<EmployeeLeave> EmployeeLeaveEmployees { get; set; } = new List<EmployeeLeave>();

    public virtual EmployeePosition EmployeePosition { get; set; } = null!;

    public virtual ICollection<EmployeeSchedule> EmployeeScheduleCreatedByEmployees { get; set; } = new List<EmployeeSchedule>();

    public virtual ICollection<EmployeeSchedule> EmployeeScheduleEmployees { get; set; } = new List<EmployeeSchedule>();

    public virtual ICollection<EmployeeSchedule> EmployeeScheduleModifiedByEmployees { get; set; } = new List<EmployeeSchedule>();

    public virtual EmployeeStatistic EmployeeStatistics { get; set; } = null!;

    public virtual ICollection<Employee> InverseSupervisor { get; set; } = new List<Employee>();

    public virtual ICollection<News> NewsCreatedByEmployees { get; set; } = new List<News>();

    public virtual ICollection<News> NewsModifiedByEmployees { get; set; } = new List<News>();

    public virtual ICollection<PostRentalReport> PostRentalReports { get; set; } = new List<PostRentalReport>();

    public virtual ICollection<Rental> RentalFinishedByEmployees { get; set; } = new List<Rental>();

    public virtual RentalPlace RentalPlace { get; set; } = null!;

    public virtual ICollection<Rental> RentalStartedByEmployees { get; set; } = new List<Rental>();

    public virtual Employee? Supervisor { get; set; }
}
