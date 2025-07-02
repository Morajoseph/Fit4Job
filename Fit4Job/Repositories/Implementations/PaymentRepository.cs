namespace Fit4Job.Repositories.Implementations
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(Fit4JobDbContext context) : base(context)
        {

        }

        public async Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            return await _context.Payments
                .Where(p => p.PaymentStatus == status)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaymentsAmountAsync()
        {
            return await _context.Payments
                .Where(p => p.PaymentStatus == PaymentStatus.completed)
                .SumAsync(p => p.Amount);
        }
    }
}
