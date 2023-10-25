
namespace Totira.Bussiness.UserService.Handlers.Commands
{
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using System.Linq.Expressions;
    using Totira.Bussiness.UserService.Commands;
    using Totira.Bussiness.UserService.Domain;
    using Totira.Bussiness.UserService.Events;
    using Totira.Support.Application.Messages;
    using static Totira.Support.Application.Messages.IMessageHandler;
    using static Totira.Support.Persistance.IRepository;

    public class UpdateTenantApplicationTypeCommandHandler : IMessageHandler<UpdateTenantApplicationTypeCommand>
    {
        private readonly ILogger<UpdateTenantApplicationTypeCommandHandler> _logger;
        private readonly IRepository<TenantApplicationType, Guid> _tenantApplicationTypeRepository;
        public UpdateTenantApplicationTypeCommandHandler(ILogger<UpdateTenantApplicationTypeCommandHandler> logger, IRepository<TenantApplicationType, Guid> tenantApplicationTypeRepository)
        {
            _logger = logger;
            _tenantApplicationTypeRepository = tenantApplicationTypeRepository;
        }

        public async Task HandleAsync(IContext context, UpdateTenantApplicationTypeCommand command)
        {
            _logger.LogDebug($"Update tenant application type for tenant {command.TenantId}");

            Expression<Func<TenantApplicationType, bool>> expression = (r => r.TenantId == command.TenantId);

            var actualData = (await _tenantApplicationTypeRepository.Get(expression)).FirstOrDefault();

            if (actualData is null)
                return;

            actualData.ApplicationType = command.ApplicationType;
    
            await _tenantApplicationTypeRepository.Update(actualData);

            var tenantUpdatedEvent = new TenantApplicationTypeUpdatedEvent(actualData.Id);
        }
    }
}
