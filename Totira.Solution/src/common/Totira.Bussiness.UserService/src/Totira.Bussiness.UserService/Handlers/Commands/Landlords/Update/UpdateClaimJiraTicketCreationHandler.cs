using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Update;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.Events.Landlord.UpdatedEvents;
using Totira.Support.Application.Messages;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands.Landlords.Update;

public class UpdateClaimJiraTicketCreationHandler : BaseMessageHandler<UpdateClaimJiraTicketCreationCommand, ClaimJiraTicketCreationUpdatedEvent>
{
    private readonly IRepository<LandlordPropertyClaim, Guid> _claimRepository;
    private readonly ILogger<UpdateClaimJiraTicketCreationHandler> _logger;
    private readonly IContextFactory _contextFactory;
    private readonly IMessageService _messageService;

    public UpdateClaimJiraTicketCreationHandler(
        ILogger<UpdateClaimJiraTicketCreationHandler> logger, 
        IContextFactory contextFactory, 
        IMessageService messageService,
        IRepository<LandlordPropertyClaim, Guid> claimRepository) : base(logger, contextFactory, messageService)
    {
        _claimRepository = claimRepository; 
        _logger = logger;
        _contextFactory = contextFactory;
        _messageService = messageService;
    }

    protected override async Task<ClaimJiraTicketCreationUpdatedEvent> Process(IContext context, UpdateClaimJiraTicketCreationCommand command)
    {
        var claimToUpdate = await _claimRepository.GetByIdAsync(command.ClaimId);

        if (claimToUpdate is null)
            throw new Exception("Property claim does not exist.");

        claimToUpdate.UpdatedOn = DateTimeOffset.UtcNow;
        claimToUpdate.HasJiraTicket = true;

        await _claimRepository.Update(claimToUpdate);

        return new ClaimJiraTicketCreationUpdatedEvent(command.ClaimId);
    }
}
