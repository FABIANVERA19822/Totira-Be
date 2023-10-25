using Microsoft.Extensions.Logging;
using Totira.Support.Application.Messages;

namespace Totira.Support.Application.Transactions
{
    public class IdempotencyService : IIdempotencyService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<IdempotencyService> _logger;

        public IdempotencyService(
            ITransactionRepository transactionRepository,
            IDateTimeProvider dateTimeProvider,
            ILogger<IdempotencyService> logger)
        {
            _transactionRepository = transactionRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task RegisterTransactionAsync(IContext context)
        {
            var transaction = new TransactionRecord(
                context.TransactionId,
                context.CreatedBy,
                context.CreatedOn,
                _dateTimeProvider.Now);

            await _transactionRepository.AddAsync(transaction);

            _logger.LogDebug($"Registered transaction with id {transaction.TransactionId}");
        }

        public async Task<bool> VerifyTransactionAsync(Guid transactionId)
        {
            var transaction = await _transactionRepository.GetAsync(transactionId);

            if (transaction == null)
                _logger.LogDebug($"Transaction {transactionId} was not yet processed");
            else
                _logger.LogWarning($"Transaction {transactionId} was already processed");

            return transaction != null;
        }
    }
}
