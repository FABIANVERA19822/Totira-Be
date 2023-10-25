using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.PropertiesService.Enums;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Create;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Update;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Events.Landlord.CreatedEvents;
using Totira.Bussiness.UserService.Events.Landlord.UpdatedEvents;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands.Landlords.Update;

public class UpdateClaimJiraTicketResultHandler : BaseMessageHandler<UpdateClaimJiraTicketResultCommand, ClaimJiraTicketResultUpdatedEvent>
{
    private readonly IRepository<LandlordPropertyClaim, Guid> _claimRepository;
    private readonly ILogger<UpdateClaimJiraTicketResultHandler> _logger;
    private readonly IContextFactory _contextFactory;
    private readonly IMessageService _messageService;
    private readonly IEventBus _bus;

    public UpdateClaimJiraTicketResultHandler(
        IRepository<LandlordPropertyClaim, Guid> claimRepository,
        ILogger<UpdateClaimJiraTicketResultHandler> logger,
        IContextFactory contextFactory,
        IMessageService messageService,
        IEventBus bus) : base(logger, contextFactory, messageService)
    {
        _claimRepository = claimRepository;
        _logger = logger;
        _contextFactory = contextFactory;
        _messageService = messageService;
        _bus = bus;
    }

    protected async override Task<ClaimJiraTicketResultUpdatedEvent> Process(IContext context, UpdateClaimJiraTicketResultCommand command)
    {
        var claimToUpdate = await _claimRepository.GetByIdAsync(command.ClaimId);

        if (claimToUpdate is null)
            throw new Exception("Property claim does not exist.");

        claimToUpdate.Status = command.Status;
        claimToUpdate.Reason = command.Reason;

        await _claimRepository.Update(claimToUpdate);

        return new ClaimJiraTicketResultUpdatedEvent(command.ClaimId);
    }
}
