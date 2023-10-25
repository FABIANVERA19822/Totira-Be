namespace Totira.Support.Application.Transactions
{
    public interface ITransactionRepository
    {

        Task AddAsync(TransactionRecord transactionRecord);


        Task<TransactionRecord> GetAsync(Guid transactionId);


        Task CommitAsync();
    }
}
