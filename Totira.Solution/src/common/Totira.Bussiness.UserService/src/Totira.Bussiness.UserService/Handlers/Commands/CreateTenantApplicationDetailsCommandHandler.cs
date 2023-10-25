using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantApplicationDetailsCommandHandler : IMessageHandler<CreateTenantApplicationDetailsCommand>
    {
        private readonly IRepository<TenantApplicationDetails, Guid> _tenantApplicationDetailsRepository;
        private readonly ILogger<CreateTenantApplicationDetailsCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public CreateTenantApplicationDetailsCommandHandler(
            IRepository<TenantApplicationDetails, Guid> tenantApplicationDetailsRepository,
            ILogger<CreateTenantApplicationDetailsCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantApplicationDetailsRepository = tenantApplicationDetailsRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }
        public async Task HandleAsync(IContext context, Either<Exception, CreateTenantApplicationDetailsCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("creating tenant application details information with id {Id}", cmd.Id);

                var cars = new List<Domain.ApplicationDetailCar>();
                var pets = new List<Domain.ApplicationDetailPet>();
                if (cmd.Cars != null && cmd.Cars.Any())
                    cmd.Cars.ForEach(c => cars.Add(new Domain.ApplicationDetailCar(c.Plate, c.Year, c.Make, c.Model)));

                if (cmd.Pets != null && cmd.Pets.Any())
                    cmd.Pets.ForEach(p => pets.Add(new Domain.ApplicationDetailPet(p.Type, p.Description, p.Size)));


                var tenantApplicationDetails = new TenantApplicationDetails()
                {
                    Id = new Guid(),
                    TenantId = cmd.Id,
                    Smoker = cmd.Smoker,
                    EstimatedMove = cmd.EstimatedMove != null ? new Domain.ApplicationDetailEstimatedMove(cmd.EstimatedMove.Month, cmd.EstimatedMove.Year) : null,
                    CreatedOn = DateTimeOffset.Now,
                    EstimatedRent = cmd.EstimatedRent,
                    Occupants = new Domain.ApplicationDetailOccupants(cmd.Occupants.Adults, cmd.Occupants.Children),
                    Cars = cars.Any() ? cars : null,
                    Pets = pets.Any() ? pets : null,
                };

                await _tenantApplicationDetailsRepository.Add(tenantApplicationDetails);

                var objectEvent = new TenantApplicationDetailsCreatedEvent(tenantApplicationDetails.Id);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
            }, async ex => {
                var objectEvent = new TenantApplicationDetailsCreatedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
                throw ex;
            });
        }
    }
}

