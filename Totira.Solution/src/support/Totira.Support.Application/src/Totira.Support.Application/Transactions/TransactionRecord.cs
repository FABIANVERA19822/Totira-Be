using System.Diagnostics.CodeAnalysis;

namespace Totira.Support.Application.Transactions
{
    public class TransactionRecord : IEquatable<TransactionRecord>
    {
        public Guid TransactionId { get; }

        public Guid CreatedBy { get; }

        public DateTimeOffset CreatedOn { get; }

        public DateTimeOffset ExecutedOn { get; }

        public TransactionRecord(Guid transactionId, Guid createdBy, DateTimeOffset createdOn, DateTimeOffset executedOn)
        {
            TransactionId = transactionId;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            ExecutedOn = executedOn;
        }

        public bool Equals([AllowNull] TransactionRecord other)
        {
            if (other == null)
                return false;

            return TransactionId == other.TransactionId
                && CreatedBy == other.CreatedBy
                && CreatedOn == other.CreatedOn
                && ExecutedOn == other.ExecutedOn;
        }
    }
}
