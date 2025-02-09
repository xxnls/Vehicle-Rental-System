using API.Models.Customers;
using API.Models.Employees;
using API.Models.Vehicles;
using System;
using System.Collections.Generic;
using API.Interfaces;

namespace API.Models.Rentals
{
    public enum RentalStatus
    {
        AwaitingPickup,
        InProgress,
        Finished,
        Cancelled
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded 
    }

    public enum DepositStatus
    {
        Pending,
        NotTaken,
        PartiallyRefunded,
        FullyRefunded,
        AppliedToCost
    }

    public partial class Rental : IBaseModel
    {
        public int RentalId { get; set; }

        public int? PostRentalReportId { get; set; }

        public int CustomerId { get; set; }

        public int VehicleId { get; set; }

        public int StartedByEmployeeId { get; set; }

        public int? FinishedByEmployeeId { get; set; }

        public string? RentalStatus { get; set; }

        public string? PaymentStatus { get; set; }

        public string? DepositStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? PickupDateTime { get; set; }

        public DateTime? FinishDateTime { get; set; }

        public decimal DepositAmount { get; set; }

        public decimal? DepositRefundAmount { get; set; }

        public decimal Cost { get; set; }

        public decimal? FinalCost { get; set; }

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

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}