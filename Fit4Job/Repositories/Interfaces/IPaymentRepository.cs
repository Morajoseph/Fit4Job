using Fit4Job.Repositories.Generic;

namespace Fit4Job.Repositories.Interfaces
{
    public interface IPaymentRepository:IGenericRepository<Payment>
    {
        Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId);
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<decimal> GetTotalPaymentsAmountAsync();
    }
}
