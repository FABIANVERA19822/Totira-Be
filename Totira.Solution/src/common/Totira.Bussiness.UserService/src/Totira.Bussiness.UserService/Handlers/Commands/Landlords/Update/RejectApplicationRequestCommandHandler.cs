using Microsoft.Extensions.Logging;
using Totira.Bussiness.PropertiesService.Enums;
using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class RejectApplicationRequestCommandHandler : BaseMessageHandler<RejectApplicationRequestCommand, ApplicationRequestRejectedEvent>
{
    private readonly IRepository<TenantPropertyApplication, Guid> _tenantPropertyApplicationRepository;
    public RejectApplicationRequestCommandHandler(
        ILogger logger,
        IContextFactory contextFactory,
        IMessageService messageService,
        IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository)
        : base(logger, contextFactory, messageService)
    {
        _tenantPropertyApplicationRepository = tenantPropertyApplicationRepository;
    }

    protected override async Task<ApplicationRequestRejectedEvent> Process(IContext context, RejectApplicationRequestCommand command)
    {
        var coincidences = await _tenantPropertyApplicationRepository.Get(item =>
            item.ApplicantId == command.TenantId &&
            item.Status == TenantPropertyApplicationStatusEnum.UnderRevision.GetEnumDescription() &&
            item.ApplicationRequestId == command.ApplicationRequestId &&
            item.PropertyId == command.PropertyId);

        if (coincidences is null || !coincidences.Any())
            throw new Exception("Property application does not exist.");

        var propertyApplication = coincidences.First();

        propertyApplication.Status = TenantPropertyApplicationStatusEnum.Rejected.GetEnumDescription();
        propertyApplication.UpdatedOn = DateTimeOffset.Now;

        await _tenantPropertyApplicationRepository.Update(propertyApplication);

        return new ApplicationRequestRejectedEvent(propertyApplication.Id);
    }
}
