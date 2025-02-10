using API.Context;
using API.Models.Rentals;
using Microsoft.EntityFrameworkCore;

namespace API.BusinessLogic
{
    public class TransactionHandler : ITransactionHandler
    {
        private readonly ApiDbContext _context;

        public TransactionHandler(ApiDbContext context)
        {
            _context = context;
        }

        public async Task HandlePaymentAsync(int rentId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var rent = await _context.Rentals
                    .Include(r => r.Payments) // Include payments related to the rent
                    .FirstOrDefaultAsync(r => r.RentalId == rentId);

                if (rent == null)
                {
                    throw new ArgumentException($"Rent with ID {rentId} not found.");
                }

                // Calculate total paid amount, where transaction status is Completed
                var totalPaid = rent.Payments
                    .Where(p => p.TransactionStatus == "Completed")
                    .Sum(p => p.Amount);

                // Firstly, check if rent has DamageFeePaymentStatus as Pending
                if (rent.DamageFeePaymentStatus == PaymentStatus.Pending.ToString())
                {//Math.Min(rent.DepositRefundAmount ?? 0, rent.DepositAmount)
                    if (totalPaid >= rent.FinalCost - (rent.DepositAmount - rent.DepositRefundAmount))
                    {
                        rent.DamageFeePaymentStatus = PaymentStatus.Completed.ToString();
                    }
                    else
                    {
                        rent.DamageFeePaymentStatus = PaymentStatus.Pending.ToString();
                    }
                }
                else
                {
                    // Check if total paid is greater than or equal to the cost + deposit amount
                    if (totalPaid >= (rent.Cost + rent.DepositAmount))
                    {
                        rent.PaymentStatus = PaymentStatus.Completed.ToString();

                        // TODO: Handle overpayment (create a refund record, update customer balance, etc.)
                        //if (totalPaid > rent.Cost)
                        //{
                        //    var overpayment = totalPaid - rent.Cost;
                        //}
                    }
                    else
                    {
                        rent.PaymentStatus = PaymentStatus.Pending.ToString();
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("Error handling payment.", ex);
            }
        }
    }

    public interface ITransactionHandler 
    {
        Task HandlePaymentAsync(int rentId);
    }
}