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
    public class CreateTenantBasicInformationCommandHandler : IMessageHandler<CreateTenantBasicInformationCommand>
    {
        private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
        private readonly ILogger<CreateTenantBasicInformationCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public CreateTenantBasicInformationCommandHandler(
            IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
            ILogger<CreateTenantBasicInformationCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantBasicInformationRepository = tenantBasicInformationRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, CreateTenantBasicInformationCommand command)
        {
            _logger.LogInformation("creating tenant personal information with id {tenantId}", command.Id);
            Guid? messageOutboxId = null;

            var birthday = command.Birthday is null
                ? default
                : TenantBasicInformation.TenantBirthday.From(
                    command.Birthday.Year,
                    command.Birthday.Day,
                    command.Birthday.Month);

            var tenantBasicInformation = TenantBasicInformation.Create(
                command.Id,
                command.FirstName,
                command.LastName,
                birthday,
                command.SocialInsuranceNumber,
                command.AboutMe);

            await _tenantBasicInformationRepository.Add(tenantBasicInformation);

            var userCreatedEvent = new TenantPersonalInformationCreatedEvent(tenantBasicInformation.Id);
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            messageOutboxId = await _messageService.SendAsync(notificationContext, userCreatedEvent);
        }


    }
}
