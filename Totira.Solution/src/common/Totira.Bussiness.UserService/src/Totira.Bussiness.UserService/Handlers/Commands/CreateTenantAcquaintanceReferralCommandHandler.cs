using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantAcquaintanceReferralCommandHandler : IMessageHandler<CreateTenantAcquaintanceReferralCommand>
    {
        private readonly ILogger<CreateTenantAcquaintanceReferralCommandHandler> _logger;
        private readonly IRepository<TenantAcquaintanceReferrals, Guid> _tenantAcquaintanceReferralsRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;

        public CreateTenantAcquaintanceReferralCommandHandler(
            IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepository,
            IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
            ILogger<CreateTenantAcquaintanceReferralCommandHandler> logger,
            IEmailHandler emailHandler,
            IOptions<FrontendSettings> settings)
        {
            _logger = logger;
            _tenantAcquaintanceReferralsRepository = tenantAcquaintanceReferralsRepository;
            _tenantBasicInformationRepository = tenantBasicInformationRepository;
            _emailHandler = emailHandler;
            _settings = settings;
        }

        public async Task HandleAsync(IContext context, CreateTenantAcquaintanceReferralCommand command)
        {
            _logger.LogDebug($"creating tenant acquaintance referral for tenant {command.TenantId}");

            var basicInformation = await _tenantBasicInformationRepository.GetByIdAsync(command.TenantId);
            if (basicInformation is null)
            {
                _logger.LogError($"Tenant with id {command.TenantId} don't exist");
                throw new Exception("Error missing tenantId for basic information");
            }
            var tenantName = $"{basicInformation.FirstName} {basicInformation.LastName}";
            var referralId = Guid.NewGuid();

            var referral = new List<TenantAcquaintanceReferral>();
            referral.Add(new TenantAcquaintanceReferral()
            {
                Id = referralId,
                FullName = command.FullName,
                TenantName = tenantName,
                Email = command.Email,
                Phone = new UserService.Domain.ContactInformationPhoneNumber(command.PhoneNumber.Number, command.PhoneNumber.CountryCode),
                Status = "Created",
                Relationship = command.Relationship,
                OtherRelationship = command.OtherRelationship,
                CreatedOn = DateTimeOffset.Now,
                TenantId = command.TenantId
            });

            var tenantAcquaintanceReferrals = new TenantAcquaintanceReferrals()
            {
                Id = command.TenantId,
                Referrals = referral
            };
            var tenant = new TenantBasicInformation()
            {
                Id = command.TenantId
            };

            try
            {
                await _tenantAcquaintanceReferralsRepository.Add(tenantAcquaintanceReferrals);
                tenant = await _tenantBasicInformationRepository.GetByIdAsync(command.TenantId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            // Now we have to send the email with the required info
            var subject = "Acquaintance Referral";
            var tenantFullName = $"{tenant.FirstName} {tenant.LastName}";

            var link = EmailHelper.BuildTenantAcquaintanceReferralFeedbackLink(
                baseUrl: _settings.Value.Url,
                referralId: referralId,
                tenantId: command.TenantId);

            var emailBody = EmailHelper.BuildTenantAcquaintanceReferralEmailBody(command.FullName, tenantFullName, link);

            var isSent = await _emailHandler.SendEmailAsync(command.Email, subject, emailBody);
            if (!isSent)
                _logger.LogError("Fail sending email.");
        }
    }
}
