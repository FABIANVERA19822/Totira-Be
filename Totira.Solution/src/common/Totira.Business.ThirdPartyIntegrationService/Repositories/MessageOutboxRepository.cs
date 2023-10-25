using Totira.Support.TransactionalOutbox;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Repositories
{
    public class MessageOutboxRepository : IMessageRepository
    {
        private readonly IRepository<MessageOutbox, Guid> _repository;

        public MessageOutboxRepository(IRepository<MessageOutbox, Guid> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(IPersistedMessage messageOutbox)
        {
            await _repository.Add((MessageOutbox)messageOutbox);
        }

        public async Task<IPersistedMessage> GetAsync(Guid messageId)
        {
            return await _repository.GetByIdAsync(messageId);
        }

        public async Task<IEnumerable<IPersistedMessage>> GetPendingAsync(int count)
        {
            var list = await _repository.Get(m => m.Id != Guid.Empty);
            return list.Take(count).ToList();
        }

        public async Task DeleteAsync(IPersistedMessage messageOutbox)
        {

            await _repository.Delete((MessageOutbox)messageOutbox);
        }

        public async Task CommitAsync()
        {
        }

    }
}
