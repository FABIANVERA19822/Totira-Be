namespace Totira.Support.TransactionalOutbox
{

    public interface IMessageRepository
    {
        Task<IPersistedMessage> GetAsync(Guid messageId);
        Task<IEnumerable<IPersistedMessage>> GetPendingAsync(int count);
        Task AddAsync(IPersistedMessage persistedMessage);
        Task DeleteAsync(IPersistedMessage persistedMessage);
        Task CommitAsync();
    }
}
