

using Newtonsoft.Json;

namespace Totira.Support.Application.Messages
{
    public class Context : IContext
    {

        public Guid TransactionId { get; }

        public Guid CreatedBy { get; }

        public DateTimeOffset CreatedOn { get; }

        public string Href { get; }
        

        [JsonConstructor]
        internal Context(Guid transactionId, Guid createdBy, DateTimeOffset createdOn, string href)
        {
            TransactionId = transactionId;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            Href = href;            
        }
    }
}
