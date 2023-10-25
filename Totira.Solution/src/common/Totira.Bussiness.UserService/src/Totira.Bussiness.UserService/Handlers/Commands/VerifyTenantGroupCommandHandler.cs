using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class VerifyTenantGroupCommandHandler : IMessageHandler<VerifyTenantGroupCommand>
{
    private readonly ILogger<VerifyTenantGroupCommandHandler> _logger;
    private readonly IRepository<TenantGroupVerificationProfile, Guid> _tenantGroupValidationRepository;
    private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
    private readonly IContextFactory _contextFactory;
    private readonly IMessageService _messageService;

    public VerifyTenantGroupCommandHandler(
        ILogger<VerifyTenantGroupCommandHandler> logger,
        IRepository<TenantGroupVerificationProfile, Guid> tenantGroupValidationRepository,
        IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
        IContextFactory contextFactory,
             IMessageService messageService)
    {
        _logger = logger;
        _tenantGroupValidationRepository = tenantGroupValidationRepository;
        _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
        _contextFactory = contextFactory;
        _messageService = messageService;
    }

    public async Task HandleAsync(IContext context, Either<Exception, VerifyTenantGroupCommand> command)
    {
        await command.MatchAsync(async cmd => {
            var tenantGroupVerification = await _tenantGroupValidationRepository.GetByIdAsync(cmd.MainTenantId);
            
            if (tenantGroupVerification is not null)
            {
                if (cmd.CompleteVerification)
                    tenantGroupVerification.CompleteInitialVerification();

                if (cmd.RequestReVerification)
                    tenantGroupVerification.InitReVerification();
                
                if (cmd.CompleteReVerification)
                    tenantGroupVerification.CompleteReVerification();

                await _tenantGroupValidationRepository.Update(tenantGroupVerification);
            }
            else
            {    
                tenantGroupVerification = TenantGroupVerificationProfile.Empty(cmd.MainTenantId);

                if (cmd.RequestVerification)
                    tenantGroupVerification.InitVerification();

                await _tenantGroupValidationRepository.Add(tenantGroupVerification);
            }

            var tenantGroupVerifiedEvent = new TenantGroupVerificationDoneEvent(cmd.MainTenantId);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantGroupVerifiedEvent);
        }, async ex => {
            var tenantGroupVerifiedEvent = new TenantGroupVerificationDoneEvent(Guid.Empty);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantGroupVerifiedEvent);
            throw ex;
        });
    }
}
