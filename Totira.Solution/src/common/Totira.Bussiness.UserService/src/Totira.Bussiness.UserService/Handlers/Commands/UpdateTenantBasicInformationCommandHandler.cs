using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Bussiness.UserService.Domain.TenantBasicInformation;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{

    public class UpdateTenantBasicInformationCommandHandler : IMessageHandler<UpdateTenantBasicInformationCommand>
    {
        private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
        private readonly ILogger<UpdateTenantBasicInformationCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public UpdateTenantBasicInformationCommandHandler(
            IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
            ILogger<UpdateTenantBasicInformationCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantBasicInformationRepository = tenantBasicInformationRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }



        public async Task HandleAsync(IContext context, UpdateTenantBasicInformationCommand command)
        {
            _logger.LogInformation("Updating tenant personal information with id {tenantId}", command.Id);
            Guid? messageOutboxId = null;

            var actualData = await _tenantBasicInformationRepository.GetByIdAsync(command.Id);

            if (actualData is null)
                return;

            var birthday = TenantBirthday.From(
                command.Birthday.Year,
                command.Birthday.Day,
                command.Birthday.Month);

            actualData.UpdateInformation(
                command.FirstName,
                command.LastName,
                birthday,
                command.SocialInsuranceNumber!,
                command.AboutMe);

            await _tenantBasicInformationRepository.Update(actualData);

            var message = "Refresh Page";
            var tenantUpdatedEvent = new TenantPersonalInformationUpdatedEvent(actualData.Id, message);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);
        }
    }
}
