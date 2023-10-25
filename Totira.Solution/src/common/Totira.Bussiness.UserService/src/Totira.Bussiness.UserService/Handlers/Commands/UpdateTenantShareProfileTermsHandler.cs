using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;


public class UpdateTenantShareProfileTermsHandler : IMessageHandler<UpdateTenantShareProfileTermsCommand>
{
    private readonly ILogger<UpdateTenantShareProfileTermsHandler> _logger;
    private readonly IRepository<TenantShareProfile, Guid> _tenantShareProfileRepository;
    private readonly IContextFactory _contextFactory;
    private readonly IMessageService _messageService;
    public UpdateTenantShareProfileTermsHandler(
        ILogger<UpdateTenantShareProfileTermsHandler> logger,
        IRepository<TenantShareProfile, Guid> tenantShareProfileRepository,
        IContextFactory contextFactory,
        IMessageService messageService)
    {
        _logger = logger;
        _tenantShareProfileRepository = tenantShareProfileRepository;
        _contextFactory = contextFactory;
        _messageService = messageService;
    }
    public async Task HandleAsync(IContext context, UpdateTenantShareProfileTermsCommand command)
    {
        _logger.LogDebug($"updating  tenant share profile terms and conditions {command.Id}");

        var tenantShareProfile = (await _tenantShareProfileRepository.Get(item => item.TenantId == command.Id)).FirstOrDefault();

        if (tenantShareProfile != null)
        {
            tenantShareProfile.AcceptTermsAndConditions();
            await _tenantShareProfileRepository.Update(tenantShareProfile);

            var userCreatedEvent = new TenantEmploymentReferenceCreatedEvent(tenantShareProfile.Id);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
        }
    }
}


