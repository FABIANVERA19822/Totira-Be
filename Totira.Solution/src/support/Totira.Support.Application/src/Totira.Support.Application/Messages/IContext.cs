namespace Totira.Support.Application.Messages
{
    public interface IContext
    {        
        Guid TransactionId { get; }
        Guid CreatedBy { get; }
        DateTimeOffset CreatedOn { get; }
        public string Href { get; }
    }
}
