namespace Totira.Support.Application.Messages
{
    public class ContextFactory : IContextFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public ContextFactory(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public IContext Create()
        {
            return Create(null, Guid.Empty);
        }

        public IContext Create(string href, Guid userId)
        {
            var transactionId = Guid.NewGuid();         

            return new Context(transactionId, userId, _dateTimeProvider.Now, href ?? string.Empty);
        }
    }
}
