using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Rental
{
    public int RentalId { get; set; }

    public int? PostRentalReportId { get; set; }

    public int CustomerId { get; set; }

    public int VehicleId { get; set; }

    public int StartedByEmployeeId { get; set; }

    public int? FinishedByEmployeeId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime? ActualStartDate { get; set; }

    public DateTime? ActualEndDate { get; set; }

    public decimal Cost { get; set; }

    public decimal? ActualCost { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Employee? FinishedByEmployee { get; set; }

    public virtual PostRentalReport? PostRentalReport { get; set; }

    public virtual Employee StartedByEmployee { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
