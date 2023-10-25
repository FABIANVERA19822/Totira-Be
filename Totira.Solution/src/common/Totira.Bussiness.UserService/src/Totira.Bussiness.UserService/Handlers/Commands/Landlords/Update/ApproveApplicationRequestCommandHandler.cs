using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Events;
using Totira.Bussiness.UserService.Extensions;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands.Landlords.Update;

public class ApproveApplicationRequestCommandHandler : BaseMessageHandler<ApproveApplicationRequestCommand, ApplicationRequestApprovedEvent>
{
    private readonly IRepository<TenantPropertyApplication, Guid> _tenantPropertyApplicationRepository;

    public ApproveApplicationRequestCommandHandler(
        ILogger logger,
        IContextFactory contextFactory,
        IMessageService messageService,
        IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository)
        : base(logger, contextFactory, messageService)
    {
        _tenantPropertyApplicationRepository = tenantPropertyApplicationRepository;
    }

    protected override async Task<ApplicationRequestApprovedEvent> Process(IContext context, ApproveApplicationRequestCommand command)
    {
        var coincidences = await _tenantPropertyApplicationRepository.Get(item =>
            item.ApplicantId == command.TenantId &&
            item.Status == TenantPropertyApplicationStatusEnum.UnderRevision.GetEnumDescription() &&
            item.ApplicationRequestId == command.ApplicationRequestId &&
            item.PropertyId == command.PropertyId);

        if (coincidences is null || !coincidences.Any())
            throw new Exception("Property application does not exist.");

        var propertyApplication = coincidences.First();

        propertyApplication.Status = TenantPropertyApplicationStatusEnum.Approved.GetEnumDescription();
        propertyApplication.UpdatedOn = DateTimeOffset.Now;

        await _tenantPropertyApplicationRepository.Update(propertyApplication);

        return new ApplicationRequestApprovedEvent(propertyApplication.Id);
    }
}
