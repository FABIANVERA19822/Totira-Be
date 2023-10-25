using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantFeedbackViaLandlordCommandHandler : IMessageHandler<CreateTenantFeedbackViaLandlordCommand>
    {
        private readonly IRepository<TenantFeedbackViaLandlord, Guid> _tenantFeedbackViaLandlordRepository;
        private readonly IRepository<TenantRentalHistories, Guid> _tenantRentalHistoriesRepository;
        private readonly ILogger<CreateTenantFeedbackViaLandlordCommandHandler> _logger;

        public CreateTenantFeedbackViaLandlordCommandHandler(
            IRepository<TenantFeedbackViaLandlord, Guid> tenantFeedbackViaLandlordRepository,
            IRepository<TenantRentalHistories, Guid> tenantRentalHistoriesRepository,
            ILogger<CreateTenantFeedbackViaLandlordCommandHandler> logger
            )
        {
            _tenantFeedbackViaLandlordRepository = tenantFeedbackViaLandlordRepository;
            _tenantRentalHistoriesRepository = tenantRentalHistoriesRepository;
            _logger = logger;
        }

        public async Task HandleAsync(IContext context, CreateTenantFeedbackViaLandlordCommand command)
        {
            _logger.LogDebug($"creating tenant {command.TenantId} feedback by landlord with id {command.LandlordId}");

            var TenantFeedbackViaLandlord = new TenantFeedbackViaLandlord()
            {
                Id = Guid.NewGuid(),
                TenantId = command.TenantId,
                LandlordId = command.LandlordId,
                Score = command.StarScore,
                Comment = command.Comment,
                CreatedOn = DateTimeOffset.Now,
            };

            await _tenantFeedbackViaLandlordRepository.Add(TenantFeedbackViaLandlord);

            var rentalHistory = await _tenantRentalHistoriesRepository.GetByIdAsync(command.TenantId);

            var history = rentalHistory.RentalHistories.Where(r => r.Id == command.LandlordId).First();

            rentalHistory.RentalHistories.Remove(history);

            history.Status = "Complete";

            rentalHistory.RentalHistories.Add(history);

            await _tenantRentalHistoriesRepository.Update(rentalHistory);

            var tenantFeedbackViaLandlordCreatedEvent = new TenantFeedbackViaLandlordCreatedEvent(TenantFeedbackViaLandlord.Id);
        }
    }
}
