using Totira.Support.Application.Messages;

namespace Totira.Support.Application.Transactions
{
    public interface IIdempotencyService
    {
        Task RegisterTransactionAsync(IContext context);
        Task<bool> VerifyTransactionAsync(Guid transactionId);
    }
}
