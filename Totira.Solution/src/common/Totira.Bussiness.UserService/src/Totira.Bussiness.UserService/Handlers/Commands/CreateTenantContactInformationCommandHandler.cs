using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
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
        public async Task HandleAsync(IContext context, Either<Exception, CreateTenantContactInformationCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("creating tenant contact info for tenant {TenantId}", cmd.TenantId);

                var tenantContact = new TenantContactInformation
                {
                    Id = Guid.NewGuid(),
                    TenantId = cmd.TenantId,
                    HousingStatus = cmd.HousingStatus,
                    Country = cmd.SelectedCountry,
                    Province = cmd.Province,
                    City = cmd.City,
                    ZipCode = cmd.ZipCode,
                    StreetAddress = cmd.StreetAddress,
                    Unit = cmd.Unit,
                    Email = cmd.Email,
                    PhoneNumber = new ContactInformationPhoneNumber(cmd.PhoneNumber.Number, cmd.PhoneNumber.CountryCode),
                    CreatedOn = DateTimeOffset.Now,
                };

                await _tenantContactInfoRepository.Add(tenantContact);
            }, ex => throw ex);
        }

    }
}
