using API.Context;
using API.Models.DTOs.Rentals;
using API.Models.Rentals;
using API.Services.Customers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API.BusinessLogic;

namespace API.Services.Rentals
{
    public class PaymentsService : BaseApiService<Payment, PaymentDto, PaymentDto>
    {
        private readonly ApiDbContext _context;
        private readonly CustomersService _customersService;
        private readonly RentalsService _rentalsService;
        private readonly ITransactionHandler _transactionHandler;

        public PaymentsService(
            ApiDbContext context,
            CustomersService customersService,
            RentalsService rentalsService,
            ITransactionHandler transactionHandler) : base(context)
        {
            _context = context;
            _customersService = customersService;
            _rentalsService = rentalsService;
            _transactionHandler = transactionHandler;
        }

        protected override Expression<Func<Payment, bool>> BuildSearchQuery(string search)
        {
            return p =>
                p.PaymentId.ToString().Contains(search) ||
                p.Customer.FirstName.Contains(search) || // Example search criteria
                p.Customer.LastName.Contains(search) ||
                p.RentId.ToString().Contains(search) ||
                p.Amount.ToString().Contains(search) ||
                p.PaymentMethod.Contains(search) ||
                p.TransactionStatus.Contains(search);
        }

        #region Mapping

        public override Payment MapToEntity(PaymentDto model)
        {
            return new Payment
            {
                PaymentId = model.PaymentId,
                CustomerId = model.CustomerId,
                RentId = model.RentId,
                Amount = model.Amount,
                PaymentDate = model.PaymentDate,
                PaymentMethod = model.PaymentMethod,
                TransactionStatus = model.TransactionStatus,
                FailReason = model.FailReason,
                RefundReason = model.RefundReason,
            };
        }

        public override Expression<Func<Payment, PaymentDto>> MapToDto()
        {
            return p => new PaymentDto
            {
                PaymentId = p.PaymentId,
                CustomerId = p.CustomerId,
                Customer = p.Customer != null ? _customersService.MapSingleEntityToDto(p.Customer) : null,
                RentId = p.RentId,
                Rent = p.Rent != null ? _rentalsService.MapSingleEntityToDto(p.Rent) : null, // If you need it
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,
                TransactionStatus = p.TransactionStatus,
                FailReason = p.FailReason,
                RefundReason = p.RefundReason,
            };
        }

        public override PaymentDto MapSingleEntityToDto(Payment entity)
        {
            return new PaymentDto
            {
                PaymentId = entity.PaymentId,
                CustomerId = entity.CustomerId,
                Customer = entity.Customer != null ? _customersService.MapSingleEntityToDto(entity.Customer) : null,
                RentId = entity.RentId,
                Rent = entity.Rent != null ? _rentalsService.MapSingleEntityToDto(entity.Rent) : null,
                Amount = entity.Amount,
                PaymentDate = entity.PaymentDate,
                PaymentMethod = entity.PaymentMethod,
                TransactionStatus = entity.TransactionStatus,
                FailReason = entity.FailReason,
                RefundReason = entity.RefundReason,
            };
        }

        #endregion

        public override async Task<Payment> FindEntityById(int id)
        {
            return await _context.Payments
                .Include(p => p.Customer)
                .Include(p => p.Rent)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        protected override IQueryable<Payment> IncludeRelatedEntities(IQueryable<Payment> query)
        {
            return query
                .Include(p => p.Customer)
                .Include(p => p.Rent);
        }

        public override async Task<PaymentDto> CreateAsync(PaymentDto paymentDto)
        {
            try
            {
                var payment = new Payment 
                {
                    CustomerId = paymentDto.Customer.Id,
                    RentId = paymentDto.Rent.RentalId,
                    Amount = paymentDto.Amount, 
                    PaymentDate = paymentDto.PaymentDate,
                    PaymentMethod = paymentDto.PaymentMethod,
                    TransactionStatus = paymentDto.TransactionStatus,
                    FailReason = paymentDto.FailReason,
                    RefundReason = paymentDto.RefundReason 
                };
                
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                // Handle payment
                await _transactionHandler.HandlePaymentAsync(payment.RentId);

                return MapSingleEntityToDto(payment);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the payment.", ex);
            }
        }

        protected override void UpdateEntity(Payment entity, PaymentDto dto)
        {
            entity.Amount = dto.Amount;
            entity.PaymentDate = dto.PaymentDate;
            entity.PaymentMethod = dto.PaymentMethod;
            entity.TransactionStatus = dto.TransactionStatus;
            entity.FailReason = dto.FailReason;
            entity.RefundReason = dto.RefundReason;

            if (dto.Customer != null)
            {
                entity.CustomerId = dto.Customer.Id;
            }

            if (dto.Rent != null)
            {
                entity.RentId = dto.Rent.RentalId;
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var payment = await FindEntityById(id);
                if (payment == null)
                    return false;

                var paymentDeleted = await base.DeleteAsync(id);
                if (!paymentDeleted)
                    return false;

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
