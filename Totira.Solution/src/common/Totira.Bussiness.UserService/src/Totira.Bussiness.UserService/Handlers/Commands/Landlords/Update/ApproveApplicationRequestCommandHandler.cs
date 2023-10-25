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
        ILogger<ApproveApplicationRequestCommandHandler> logger,
        IContextFactory contextFactory,
        IMessageService messageService,
        IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository)
        : base(logger, contextFactory, messageService)
    {
        _tenantPropertyApplicationRepository = tenantPropertyApplicationRepository;
    }

    protected override async Task<ApplicationRequestApprovedEvent> Process(IContext context, ApproveApplicationRequestCommand command)
    {
        var propertyApplication = await _tenantPropertyApplicationRepository.GetByIdAsync(command.PropertyApplicationId) ?? throw new Exception("Property application does not exist.");
        propertyApplication.Status = TenantPropertyApplicationStatusEnum.Approved.GetEnumDescription();
        propertyApplication.UpdatedOn = DateTimeOffset.Now;

        await _tenantPropertyApplicationRepository.Update(propertyApplication);

        return new ApplicationRequestApprovedEvent(propertyApplication.Id);
    }
}
