using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Customers;

namespace BackOffice.Models.DTOs.Rentals
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

    public class PaymentDto : BaseDtoModel
    {
        public int PaymentId { get; set; }
        public int CustomerId { get; set; }
        public int? RentId { get; set; }

        private decimal _amount;

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _paymentDate = DateTime.Now;

        public DateTime PaymentDate
        {
            get => _paymentDate;
            set
            {
                if (_paymentDate != value)
                {
                    _paymentDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _paymentMethod;

        public string? PaymentMethod
        {
            get => _paymentMethod;
            set
            {
                if (_paymentMethod != value)
                {
                    _paymentMethod = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _transactionStatus;

        public string? TransactionStatus
        {
            get => _transactionStatus;
            set
            {
                if (_transactionStatus != value)
                {
                    _transactionStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _failReason;

        public string? FailReason
        {
            get => _failReason;
            set
            {
                if (_failReason != value)
                {
                    _failReason = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _refundReason;

        public string? RefundReason
        {
            get => _refundReason;
            set
            {
                if (_refundReason != value)
                {
                    _refundReason = value;
                    OnPropertyChanged();
                }
            }
        }

        private CustomerDto _customer;

        public CustomerDto Customer
        {
            get => _customer;
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    OnPropertyChanged();
                }
            }
        }

        private RentalDto _rent;

        public RentalDto Rent
        {
            get => _rent;
            set
            {
                if (_rent != value)
                {
                    _rent = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
