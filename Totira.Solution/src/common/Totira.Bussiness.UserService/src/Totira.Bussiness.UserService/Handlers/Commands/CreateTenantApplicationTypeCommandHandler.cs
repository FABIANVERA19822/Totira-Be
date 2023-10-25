﻿namespace Totira.Bussiness.UserService.Handlers.Commands
{
    using Microsoft.Extensions.Logging;
    using Totira.Bussiness.UserService.Commands;
    using Totira.Bussiness.UserService.Domain;
    using Totira.Bussiness.UserService.Events;
    using Totira.Support.Application.Messages;
    using Totira.Support.TransactionalOutbox;
    using static Totira.Support.Application.Messages.IMessageHandler;
    using static Totira.Support.Persistance.IRepository;

    public class CreateTenantApplicationTypeCommandHandler : IMessageHandler<CreateTenantApplicationTypeCommand>
    {
        private readonly ILogger<CreateTenantApplicationTypeCommandHandler> _logger;
        private readonly IRepository<TenantApplicationType, Guid> _tenantApplicationTypeRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public CreateTenantApplicationTypeCommandHandler(
            ILogger<CreateTenantApplicationTypeCommandHandler> logger,
            IRepository<TenantApplicationType, Guid> tenantApplicationTypeRepository,
            IContextFactory contextFactory,
             IMessageService messageService)
        { 
            _logger = logger;
            _tenantApplicationTypeRepository= tenantApplicationTypeRepository;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }


        /// <summary>
        /// Create a new applicationtype for the tenant 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(IContext context, CreateTenantApplicationTypeCommand command)
        {

            _logger.LogInformation("creating tenant application type with id {tenantId}", command.TenantId);
 
            var tenantApplicationType = new TenantApplicationType { Id= command.TenantId, TenantId = command.TenantId, ApplicationType= command.ApplicationType};
            await _tenantApplicationTypeRepository.Add(tenantApplicationType);

            var userCreatedEvent = new TenantApplicationTypeCreatedEvent(tenantApplicationType.Id);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
        }
    }
}
