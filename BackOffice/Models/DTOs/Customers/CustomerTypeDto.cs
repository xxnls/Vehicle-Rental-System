using BackOffice.Interfaces;

namespace BackOffice.Models.DTOs.Customers
{
    public class CustomerTypeDto : BaseDtoModel
    {
        public int CustomerTypeId { get; set; }

        private string _customerType = null!;
        public string CustomerType
        {
            get => _customerType;
            set
            {
                _customerType = value;
                OnPropertyChanged();
            }
        }

        private short? _maxRentals;
        public short? MaxRentals
        {
            get => _maxRentals;
            set
            {
                _maxRentals = value;
                OnPropertyChanged();
            }
        }

        private double? _discountPercent;
        public double? DiscountPercent
        {
            get => _discountPercent;
            set
            {
                _discountPercent = value;
                OnPropertyChanged();
            }
        }

    }
}
