using LanguageExt;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantContactInformationCommandHandler : IMessageHandler<UpdateTenantContactInformationCommand>
    {
        private readonly ILogger<UpdateTenantContactInformationCommandHandler> _logger;
        private readonly IRepository<TenantContactInformation, Guid> _tenantContactInfoRepository;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public UpdateTenantContactInformationCommandHandler(ILogger<UpdateTenantContactInformationCommandHandler> logger,
                                                     IRepository<TenantContactInformation, Guid> tenantContactInfoRepository,
                                                      IContextFactory contextFactory,
                                                      IMessageService messageService)
        {
            _logger = logger;
            _tenantContactInfoRepository = tenantContactInfoRepository;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }
        public async Task HandleAsync(IContext context, Either<Exception, UpdateTenantContactInformationCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("Updating tenant acquaintance referral for tenant {TenantId}", cmd.TenantId);

                Expression<Func<TenantContactInformation, bool>> predicate = contactInfo => contactInfo.TenantId == cmd.TenantId;

                var actualData = (await _tenantContactInfoRepository.Get(predicate)).FirstOrDefault();

                if (actualData != null)
                {
                    actualData.HousingStatus = string.IsNullOrEmpty(cmd.HousingStatus) ? actualData.HousingStatus : cmd.HousingStatus;
                    actualData.Country = string.IsNullOrEmpty(cmd.SelectedCountry) ? actualData.Country : cmd.SelectedCountry;
                    actualData.Province = string.IsNullOrEmpty(cmd.Province) ? actualData.Province : cmd.Province;
                    actualData.City = string.IsNullOrEmpty(cmd.City) ? actualData.City : cmd.City;
                    actualData.ZipCode = string.IsNullOrEmpty(cmd.ZipCode) ? actualData.ZipCode : cmd.ZipCode;
                    actualData.StreetAddress = string.IsNullOrEmpty(cmd.StreetAddress) ? actualData.StreetAddress : cmd.StreetAddress;
                    if (cmd.Unit.Trim() == string.Empty) actualData.Unit = string.Empty;
                    else actualData.Unit = string.IsNullOrEmpty(cmd.Unit) ? actualData.Unit : cmd.Unit;
                    actualData.PhoneNumber.Number = string.IsNullOrEmpty(cmd.PhoneNumber.Number) ? actualData.PhoneNumber.Number : cmd.PhoneNumber.Number;
                    actualData.PhoneNumber.CountryCode = string.IsNullOrEmpty(cmd.PhoneNumber.CountryCode) ? actualData.PhoneNumber.CountryCode : cmd.PhoneNumber.CountryCode;
                    actualData.Email = string.IsNullOrEmpty(cmd.Email) ? actualData.Email : cmd.Email;

                    await _tenantContactInfoRepository.Update(actualData);
                    var tenantUpdatedEvent = new TenantContactInformationUpdatedEvent(actualData.Id);
                    var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                    var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);
                }
            }, async ex => {
                var tenantUpdatedEvent = new TenantContactInformationUpdatedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);
                throw ex;
            });
        }
    }
}
