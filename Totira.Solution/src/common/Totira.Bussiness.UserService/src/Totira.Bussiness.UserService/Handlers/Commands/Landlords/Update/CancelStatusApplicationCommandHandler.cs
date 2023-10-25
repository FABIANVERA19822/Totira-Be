using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Events.Landlord.UpdatedEvents;
using Totira.Bussiness.UserService.Extensions;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands.Landlords.Update;

public class CancelStatusPropertyApplicationCommandHandler : BaseMessageHandler<CancelStatusPropertyApplicationCommand, PropertyApplicationStatusCanceledEvent>
{
    private readonly IRepository<TenantPropertyApplication, Guid> _tenantPropertyApplicationRepository;

    public CancelStatusPropertyApplicationCommandHandler(
        IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository,
        ILogger<CancelStatusPropertyApplicationCommandHandler> logger,
        IContextFactory contextFactory,
        IMessageService messageService)
        : base(logger, contextFactory, messageService)
    {
        _tenantPropertyApplicationRepository = tenantPropertyApplicationRepository;
    }

    protected override async Task<PropertyApplicationStatusCanceledEvent> Process(IContext context, CancelStatusPropertyApplicationCommand command)
    {
        var tenantPropertyApplication = await _tenantPropertyApplicationRepository.GetByIdAsync(command.PropertyApplicationId);
        if (tenantPropertyApplication is null)
        {
            _logger.LogDebug("Tenant property application with id {id} does not exist.", command.PropertyApplicationId);
            throw new Exception("Tenant property application does not exist.");
        }

        var validStatusesToBeCanceled = new List<string>()
        {
            TenantPropertyApplicationStatusEnum.Approved.GetEnumDescription(),
            TenantPropertyApplicationStatusEnum.Rejected.GetEnumDescription()
        };

        if (validStatusesToBeCanceled.Contains(tenantPropertyApplication.Status))
        {
            tenantPropertyApplication.Status = TenantPropertyApplicationStatusEnum.Canceled.GetEnumDescription();
            tenantPropertyApplication.UpdatedOn = DateTime.UtcNow;
            await _tenantPropertyApplicationRepository.Update(tenantPropertyApplication);
            _logger.LogDebug("Status changed to {status}", tenantPropertyApplication.Status);
        }
        else
        {
            _logger.LogDebug("Current property application status: {status}", tenantPropertyApplication.Status);
            throw new Exception("Property application cannot be canceled because is not approved or rejected.");
        }

        return new(command.PropertyApplicationId);
    }
}
