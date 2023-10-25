using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
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
        private readonly IOtpHandler _otpHandler;


        public CreateTenantAcquaintanceReferralCommandHandler(
            IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepository,
            IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
            ILogger<CreateTenantAcquaintanceReferralCommandHandler> logger,
            IEmailHandler emailHandler,
            IOptions<FrontendSettings> settings,
            IOtpHandler otpHandler)
        {
            _logger = logger;
            _tenantAcquaintanceReferralsRepository = tenantAcquaintanceReferralsRepository;
            _tenantBasicInformationRepository = tenantBasicInformationRepository;
            _emailHandler = emailHandler;
            _settings = settings;
            _otpHandler = otpHandler;
        }

        public async Task HandleAsync(IContext context, Either<Exception, CreateTenantAcquaintanceReferralCommand> command)
        {
            await command.MatchAsync(async cmd =>
            {
                _logger.LogDebug("creating tenant acquaintance referral for tenant {TenantId}", cmd.TenantId);

                var basicInformation = await _tenantBasicInformationRepository.GetByIdAsync(cmd.TenantId);
                if (basicInformation is null)
                {
                    _logger.LogError("Tenant with id {TenantId} don't exist", cmd.TenantId);
                    throw new Exception("Error missing tenantId for basic information");
                }
                var tenantName = $"{basicInformation.FirstName} {basicInformation.LastName}";
                var referralId = Guid.NewGuid();

                var referral = new List<TenantAcquaintanceReferral>();
                referral.Add(new TenantAcquaintanceReferral()
                {
                    Id = referralId,
                    FullName = cmd.FullName,
                    TenantName = tenantName,
                    Email = cmd.Email,
                    Phone = new(cmd.PhoneNumber.Number, cmd.PhoneNumber.CountryCode),
                    Status = "Created",
                    Relationship = cmd.Relationship,
                    OtherRelationship = cmd.OtherRelationship,
                    CreatedOn = DateTimeOffset.Now,
                    TenantId = cmd.TenantId
                });

                var tenantAcquaintanceReferrals = new TenantAcquaintanceReferrals()
                {
                    Id = cmd.TenantId,
                    Referrals = referral
                };
                var tenant = new TenantBasicInformation()
                {
                    Id = cmd.TenantId
                };

                try
                {
                    await _tenantAcquaintanceReferralsRepository.Add(tenantAcquaintanceReferrals);
                    tenant = await _tenantBasicInformationRepository.GetByIdAsync(cmd.TenantId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                // Now we have to send the email with the required info
                var subject = "Acquaintance Referral";
                var tenantFullName = $"{tenant.FirstName} {tenant.LastName}";

                var otpId = Guid.NewGuid();
                await _otpHandler.SetOtpProcessAsync(otpId, referralId, cmd.TenantId, cmd.Email, cmd.Email, "ReferralFeedback", true, false);

                var link = EmailHelper.BuildTenantAcquaintanceReferralFeedbackOtpLink(
                    baseUrl: _settings.Value.Url,
                    otpId: otpId);

                var emailBody = EmailHelper.BuildTenantAcquaintanceReferralEmailBody(cmd.FullName, tenantFullName, link);

                var isSent = await _emailHandler.SendEmailAsync(cmd.Email, subject, emailBody);
                if (!isSent)
                    _logger.LogError("Fail sending email.");

                return isSent;
            }, (Func<Exception, bool>)(ex => throw ex));
        }
    }
}