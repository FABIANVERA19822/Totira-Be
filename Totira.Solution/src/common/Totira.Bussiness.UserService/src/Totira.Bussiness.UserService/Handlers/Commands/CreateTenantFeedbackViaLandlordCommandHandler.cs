﻿using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantFeedbackViaLandlordCommandHandler : IMessageHandler<CreateTenantFeedbackViaLandlordCommand>
    {
        private readonly IRepository<TenantFeedbackViaLandlord, Guid> _tenantFeedbackViaLandlordRepository;
        private readonly IRepository<TenantRentalHistories, Guid> _tenantRentalHistoriesRepository;
        private readonly ILogger<CreateTenantFeedbackViaLandlordCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public CreateTenantFeedbackViaLandlordCommandHandler(
            IRepository<TenantFeedbackViaLandlord, Guid> tenantFeedbackViaLandlordRepository,
            IRepository<TenantRentalHistories, Guid> tenantRentalHistoriesRepository,
            ILogger<CreateTenantFeedbackViaLandlordCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantFeedbackViaLandlordRepository = tenantFeedbackViaLandlordRepository;
            _tenantRentalHistoriesRepository = tenantRentalHistoriesRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
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
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantFeedbackViaLandlordCreatedEvent);
        }
    }
}
