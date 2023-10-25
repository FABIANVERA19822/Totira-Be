using Microsoft.Extensions.Logging;
using Totira.Bussiness.PropertiesService.Enums;
using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Events;
using Totira.Bussiness.UserService.Handlers.Commands.Landlords.Update;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class RejectApplicationRequestCommandHandler : BaseMessageHandler<RejectApplicationRequestCommand, ApplicationRequestRejectedEvent>
{
    private readonly IRepository<TenantPropertyApplication, Guid> _tenantPropertyApplicationRepository;
    public RejectApplicationRequestCommandHandler(
        ILogger<RejectApplicationRequestCommandHandler> logger,
        IContextFactory contextFactory,
        IMessageService messageService,
        IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository)
        : base(logger, contextFactory, messageService)
    {
        _tenantPropertyApplicationRepository = tenantPropertyApplicationRepository;
    }

    protected override async Task<ApplicationRequestRejectedEvent> Process(IContext context, RejectApplicationRequestCommand command)
    {
        var propertyApplication = await _tenantPropertyApplicationRepository.GetByIdAsync(command.PropertyApplicationId) ?? throw new Exception("Property application does not exist.");
        propertyApplication.Status = TenantPropertyApplicationStatusEnum.Rejected.GetEnumDescription();
        propertyApplication.UpdatedOn = DateTimeOffset.Now;

        await _tenantPropertyApplicationRepository.Update(propertyApplication);

        return new ApplicationRequestRejectedEvent(propertyApplication.Id);
    }
}
