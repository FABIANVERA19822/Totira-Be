
using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateGroupApplicationCommandHandler : IMessageHandler<UpdateGroupApplicationCommand>
    {
        private readonly ILogger<UpdateGroupApplicationCommandHandler> _logger;
        private readonly IRepository<TenantGroupApplicationProfile, Guid> _tenantGroupApplicationProfileRepository;
        public UpdateGroupApplicationCommandHandler(ILogger<UpdateGroupApplicationCommandHandler> logger,
            IRepository<TenantGroupApplicationProfile, Guid> tenantGroupApplicationProfileRepository)
        {
            _logger = logger;
            _tenantGroupApplicationProfileRepository = tenantGroupApplicationProfileRepository;
        }
        public async Task HandleAsync(IContext context, Either<Exception, UpdateGroupApplicationCommand> command)
        {
            await command.MatchAsync(async cmd => {
                foreach (var applicationDto in cmd.GroupApplicationProfiles)
                {
                    _logger.LogDebug("Updating tenant Application Group profile for tenant {TenantId}", applicationDto.TenantId);
                    var actualData = await _tenantGroupApplicationProfileRepository.GetByIdAsync(applicationDto.TenantId);

                    if (actualData != null)
                    {
                        TenantGroupApplicationProfile tenantGroupApplicationProfile = new TenantGroupApplicationProfile(
                        applicationDto.TenantId, applicationDto.FirstName, applicationDto.Email, applicationDto.InvinteeType,
                        applicationDto.Status, applicationDto.IsVerifiedEmailConfirmation, DateTimeOffset.Now)
                        {
                            TenantId = applicationDto.TenantId,
                            FirstName = applicationDto.FirstName,
                            Email = applicationDto.Email,
                            InvinteeType = applicationDto.InvinteeType,
                            Status = applicationDto.Status,
                            IsVerifiedEmailConfirmation = applicationDto.IsVerifiedEmailConfirmation,
                            CreatedOn = DateTimeOffset.Now,
                        };
                    }
                    await _tenantGroupApplicationProfileRepository.Update(actualData);
                }
            }, ex => throw ex);
        }
    }
}
