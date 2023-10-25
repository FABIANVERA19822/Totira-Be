using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static System.Net.Mime.MediaTypeNames;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{ 

    public class DeleteCosignersFromGroupApplicationCommandHandler : IMessageHandler<DeleteCosignersFromGroupApplicationCommand>
    {
        private readonly ILogger<DeleteCosignersFromGroupApplicationCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public DeleteCosignersFromGroupApplicationCommandHandler(
             IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
             ILogger<DeleteCosignersFromGroupApplicationCommandHandler> logger,
            IContextFactory contextFactory,
             IMessageService messageService)
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, DeleteCosignersFromGroupApplicationCommand command)
        {
            var applicationsRequest = await _tenantApplicationRequestRepository.GetByIdAsync(command.ApplicationRequestId);

            if (applicationsRequest == null)
            {
                _logger.LogError($"The application request {command.ApplicationRequestId} does not exist ");
                return;
            }


            if (applicationsRequest?.Guarantor?.Id == command.CoSignerId)
            {



                applicationsRequest.Guarantor = null;
                await _tenantApplicationRequestRepository.Update(applicationsRequest);
            }


            if (applicationsRequest?.Coapplicants != null)
            {
                if (applicationsRequest.Coapplicants.Any(i => i.Id == command.CoSignerId))
                {
                    var coapplicantToBeRemoved = applicationsRequest.Coapplicants.First(ca => ca.Id == command.CoSignerId);



                    applicationsRequest.Coapplicants.Remove(coapplicantToBeRemoved);
                    await _tenantApplicationRequestRepository.Update(applicationsRequest);
                }
            }
            var cosignersLeaveFromGroupApplicationDeletedEvent = new TenantCosignersLeaveFromGroupApplicationDeletedEvent(command.CoSignerId);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, cosignersLeaveFromGroupApplicationDeletedEvent);

        }
    }

}
