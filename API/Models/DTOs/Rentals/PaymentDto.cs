using API.Models.DTOs.Customers;

namespace API.Models.DTOs.Rentals
{
    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer,
        PayPal,
        MultipleMethods,
        Other
    }

    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int CustomerId { get; set; }
        public int RentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? TransactionStatus { get; set; }
        public string? FailReason { get; set; }
        public string? RefundReason { get; set; }

        public CustomerDto Customer { get; set; }
        public RentalDto Rent { get; set; }
    }
}
