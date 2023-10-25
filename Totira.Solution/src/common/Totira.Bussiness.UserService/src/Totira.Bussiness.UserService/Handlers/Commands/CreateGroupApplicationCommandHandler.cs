using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateGroupApplicationCommandHandler : IMessageHandler<CreateGroupApplicationCommand>
    {
        private readonly ILogger<CreateGroupApplicationCommandHandler> _logger;
        private readonly IRepository<TenantGroupApplicationProfile, Guid> _tenantGroupApplicationProfileRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public CreateGroupApplicationCommandHandler(
            ILogger<CreateGroupApplicationCommandHandler> logger,
            IRepository<TenantGroupApplicationProfile, Guid> tenantGroupApplicationProfileRepository,
            IContextFactory contextFactory,
            IMessageService messageService)
        {
            _logger = logger;
            _tenantGroupApplicationProfileRepository = tenantGroupApplicationProfileRepository;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }
        public async Task HandleAsync(IContext context, CreateGroupApplicationCommand command)
        {

            foreach (var applicationDto in command.GroupApplicationProfiles)
            {
                _logger.LogDebug($"creating tenant Application Group profile for tenant {applicationDto.TenantId}");
                TenantGroupApplicationProfile tenantGroupApplicationProfile = new TenantGroupApplicationProfile(
                    applicationDto.TenantId, applicationDto.FirstName, applicationDto.Email, applicationDto.InvinteeType,
                    applicationDto.Status, applicationDto.IsVerifiedEmailConfirmation, DateTimeOffset.Now)
                {
                    TenantId = applicationDto.TenantId,
                    FirstName = applicationDto.FirstName,
                    Email = applicationDto.Email,
                    InvinteeType = applicationDto.InvinteeType,
                    Status = applicationDto.Status,
                    IsVerifiedEmailConfirmation = false,
                    CreatedOn = DateTimeOffset.Now,
                };

                await _tenantGroupApplicationProfileRepository.Add(tenantGroupApplicationProfile);
            }
        }
    }
}
