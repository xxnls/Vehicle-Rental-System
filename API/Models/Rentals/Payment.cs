using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using API.Models.Customers;

namespace API.Models.Rentals
{
    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer,
        PayPal,
        Other
    }

    public class Payment
    {
        public int PaymentId { get; set; }
        public int CustomerId { get; set; }
        public int? RentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionStatus { get; set; }
        public string? FailReason { get; set; }
        public string? RefundReason { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Rental Rent { get; set; } 
    }
}
