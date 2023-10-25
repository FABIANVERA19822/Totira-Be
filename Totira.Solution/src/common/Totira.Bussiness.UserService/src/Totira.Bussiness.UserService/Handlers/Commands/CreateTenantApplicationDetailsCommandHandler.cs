using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantApplicationDetailsCommandHandler : IMessageHandler<CreateTenantApplicationDetailsCommand>
    {
        private readonly IRepository<TenantApplicationDetails, Guid> _tenantApplicationDetailsRepository;
        private readonly ILogger<CreateTenantApplicationDetailsCommandHandler> _logger;
        public CreateTenantApplicationDetailsCommandHandler(
            IRepository<TenantApplicationDetails, Guid> tenantApplicationDetailsRepository,
            ILogger<CreateTenantApplicationDetailsCommandHandler> logger
            )
        {
            _tenantApplicationDetailsRepository = tenantApplicationDetailsRepository;
            _logger = logger;
        }
        public async Task HandleAsync(IContext context, CreateTenantApplicationDetailsCommand command)
        {
            _logger.LogDebug($"creating tenant application details information with id {command.Id}");

            var cars = new List<Domain.ApplicationDetailCar>();
            var pets = new List<Domain.ApplicationDetailPet>();
            if (command.Cars != null && command.Cars.Any())
                command.Cars.ForEach(c => cars.Add(new Domain.ApplicationDetailCar(c.Plate, c.Year, c.Make, c.Model)));

            if (command.Pets != null && command.Pets.Any())
                command.Pets.ForEach(p => pets.Add(new Domain.ApplicationDetailPet(p.Type, p.Description, p.Size)));


            var tenantApplicationDetails = new TenantApplicationDetails()
            {
                Id = new Guid(),
                TenantId = command.Id,
                Smoker = command.Smoker,
                EstimatedMove = command.EstimatedMove != null ? new Domain.ApplicationDetailEstimatedMove(command.EstimatedMove.Month, command.EstimatedMove.Year) : null,
                CreatedOn = DateTimeOffset.Now,
                EstimatedRent = command.EstimatedRent,
                Occupants = new Domain.ApplicationDetailOccupants(command.Occupants.Adults, command.Occupants.Children),
                Cars = cars.Any() ? cars : null,
                Pets = pets.Any() ? pets : null,
            };

            await _tenantApplicationDetailsRepository.Add(tenantApplicationDetails);

            var userCreatedEvent = new TenantApplicationDetailsCreatedEvent(tenantApplicationDetails.Id);
        }


    }
}

