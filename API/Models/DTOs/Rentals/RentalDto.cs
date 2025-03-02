﻿using API.Interfaces;
using API.Models.DTOs.Customers;
using API.Models.DTOs.Employees;
using API.Models.DTOs.Vehicles;
using API.Models.Employees;
using API.Models.Rentals;
using API.Models.Vehicles;

namespace API.Models.DTOs.Rentals
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
        FullyTaken
    }

    public class RentalDto : IBaseModel, IRentalCostCalculation
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

        public string? DamageFeePaymentStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? PickupDateTime { get; set; }

        public DateTime? FinishDateTime { get; set; }

        public decimal DepositAmount { get; set; }

        public decimal? DepositRefundAmount { get; set; }

        public decimal Cost { get; set; }

        public decimal? FinalCost { get; set; }

        public decimal? DamageFee { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public bool IsActive { get; set; }

        public CustomerDto Customer { get; set; } = null!;

        public EmployeeDto? FinishedByEmployee { get; set; }

        public PostRentalReportDto? PostRentalReport { get; set; }

        public EmployeeDto StartedByEmployee { get; set; } = null!;

        public VehicleDto Vehicle { get; set; } = null!;
    }
}
