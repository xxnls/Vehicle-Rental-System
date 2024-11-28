using API.Models.Documents;
using API.Models.Other;
using API.Models.Other.Addresses;
using API.Models.Other.News;
using API.Models.Rentals;
using System;
using System.Collections.Generic;

namespace API.Models.Employees;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int EmployeeStatisticsId { get; set; }

    public int EmployeeFinancesId { get; set; }

    public int EmployeeRoleId { get; set; }

    public int RentalPlaceId { get; set; }

    public int AddressId { get; set; }

    public int EmployeePositionId { get; set; }

    public int? SupervisorId { get; set; }

    public string Status { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

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

    public virtual EmployeeFinances EmployeeFinances { get; set; } = null!;

    public virtual ICollection<EmployeeFinances> EmployeeFinancesNavigation { get; set; } = new List<EmployeeFinances>();

    public virtual ICollection<EmployeeLeave> EmployeeLeaveApprovedBies { get; set; } = new List<EmployeeLeave>();

    public virtual ICollection<EmployeeLeave> EmployeeLeaveEmployees { get; set; } = new List<EmployeeLeave>();

    public virtual EmployeePosition EmployeePosition { get; set; } = null!;

    public virtual EmployeeRole EmployeeRole { get; set; } = null!;

    public virtual ICollection<EmployeeSchedule> EmployeeScheduleCreatedByEmployees { get; set; } = new List<EmployeeSchedule>();

    public virtual ICollection<EmployeeSchedule> EmployeeScheduleEmployees { get; set; } = new List<EmployeeSchedule>();

    public virtual ICollection<EmployeeSchedule> EmployeeScheduleModifiedByEmployees { get; set; } = new List<EmployeeSchedule>();

    public virtual EmployeeStatistics EmployeeStatistics { get; set; } = null!;

    public virtual ICollection<Employee> InverseSupervisor { get; set; } = new List<Employee>();

    public virtual ICollection<News> NewsCreatedByEmployees { get; set; } = new List<News>();

    public virtual ICollection<News> NewsModifiedByEmployees { get; set; } = new List<News>();

    public virtual ICollection<PostRentalReport> PostRentalReports { get; set; } = new List<PostRentalReport>();

    public virtual ICollection<Rental> RentalFinishedByEmployees { get; set; } = new List<Rental>();

    public virtual RentalPlace RentalPlace { get; set; } = null!;

    public virtual ICollection<Rental> RentalStartedByEmployees { get; set; } = new List<Rental>();

    public virtual Employee? Supervisor { get; set; }
}
