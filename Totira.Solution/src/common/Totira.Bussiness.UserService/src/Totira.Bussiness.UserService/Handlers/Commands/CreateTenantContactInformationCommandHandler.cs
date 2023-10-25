using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantContactInformationCommandHandler : IMessageHandler<CreateTenantContactInformationCommand>
    {
        private readonly ILogger<CreateTenantContactInformationCommandHandler> _logger;
        private readonly IRepository<TenantContactInformation, Guid> _tenantContactInfoRepository;
        public CreateTenantContactInformationCommandHandler(ILogger<CreateTenantContactInformationCommandHandler> logger,
                                                            IRepository<TenantContactInformation, Guid> tenantContactInfoRepository)
        {
            _logger = logger;
            _tenantContactInfoRepository = tenantContactInfoRepository;
        }
        public async Task HandleAsync(IContext context, CreateTenantContactInformationCommand command)
        {
            _logger.LogDebug($"creating tenant contact info for tenant {command.TenantId}");

            var tenantContact = new TenantContactInformation
            {
                Id = Guid.NewGuid(),
                TenantId = command.TenantId,
                HousingStatus = command.HousingStatus,
                Country = command.SelectedCountry,
                Province = command.Province,
                City = command.City,
                ZipCode = command.ZipCode,
                StreetAddress = command.StreetAddress,
                Unit = command.Unit,
                Email = command.Email,
                PhoneNumber = new Domain.ContactInformationPhoneNumber(command.PhoneNumber.Number, command.PhoneNumber.CountryCode),
                CreatedOn = DateTimeOffset.Now,
            };

            await _tenantContactInfoRepository.Add(tenantContact);
        }

    }
}
