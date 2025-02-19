using API.Models.Customers;
using API.Models.Employees;
using API.Models.Other;
using API.Models.Rentals;
using API.Models.Vehicles;
using System;
using System.Collections.Generic;
using API.Interfaces;

namespace API.Models.FileSystem;

public partial class Document : IBaseModel
{
    public int DocumentId { get; set; }

    public int DocumentTypeId { get; set; }

    public int DocumentCategoryId { get; set; }

    public int? VehicleId { get; set; }

    public int? EmployeeId { get; set; }

    public int? CustomerId { get; set; }

    public int? RentalPlaceId { get; set; }

    public int? RentalId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string FileName { get; set; } = null!;

    public string OriginalFileName { get; set; } = null!;

    public double FileSizeMb { get; set; }

    public byte[]? FileContent { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public int CreatedByEmployeeId { get; set; }

    public int? ModifiedByEmployeeId { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee CreatedByEmployee { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual DocumentCategory DocumentCategory { get; set; } = null!;

    public virtual DocumentType DocumentType { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual Employee? ModifiedByEmployee { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    // public virtual ICollection<LicenseApprovalRequests> LicenseApprovalRequests { get; set; } = new List<LicenseApprovalRequests>();

    public virtual Rental? Rental { get; set; }

    public virtual RentalPlace? RentalPlace { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
