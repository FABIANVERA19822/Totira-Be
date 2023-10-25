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
        public async Task HandleAsync(IContext context, UpdateTenantContactInformationCommand command)
        {
            _logger.LogDebug($"updating tenant acquaintance referral for tenant {command.TenantId}");

            Expression<Func<TenantContactInformation, bool>> predicate = contactInfo => contactInfo.TenantId == command.TenantId;

            var actualData = (await _tenantContactInfoRepository.Get(predicate)).FirstOrDefault();

            if (actualData != null)
            {
                actualData.HousingStatus = string.IsNullOrEmpty(command.HousingStatus) ? actualData.HousingStatus : command.HousingStatus;
                actualData.Country = string.IsNullOrEmpty(command.SelectedCountry) ? actualData.Country : command.SelectedCountry;
                actualData.Province = string.IsNullOrEmpty(command.Province) ? actualData.Province : command.Province;
                actualData.City = string.IsNullOrEmpty(command.City) ? actualData.City : command.City;
                actualData.ZipCode = string.IsNullOrEmpty(command.ZipCode) ? actualData.ZipCode : command.ZipCode;
                actualData.StreetAddress = string.IsNullOrEmpty(command.StreetAddress) ? actualData.StreetAddress : command.StreetAddress;
                if (command.Unit.Trim() == string.Empty) actualData.Unit = string.Empty;
                else actualData.Unit = string.IsNullOrEmpty(command.Unit) ? actualData.Unit : command.Unit;
                actualData.PhoneNumber.Number = string.IsNullOrEmpty(command.PhoneNumber.Number) ? actualData.PhoneNumber.Number : command.PhoneNumber.Number;
                actualData.PhoneNumber.CountryCode = string.IsNullOrEmpty(command.PhoneNumber.CountryCode) ? actualData.PhoneNumber.CountryCode : command.PhoneNumber.CountryCode;
                actualData.Email = string.IsNullOrEmpty(command.Email) ? actualData.Email : command.Email;

                await _tenantContactInfoRepository.Update(actualData);
                var tenantUpdatedEvent = new TenantContactInformationUpdatedEvent(actualData.Id);

                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);
            }
        }
    }
}
