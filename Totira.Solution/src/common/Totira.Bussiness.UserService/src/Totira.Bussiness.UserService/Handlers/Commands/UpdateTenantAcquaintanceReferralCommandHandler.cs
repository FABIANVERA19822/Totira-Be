using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantAcquaintanceReferralCommandHandler : IMessageHandler<UpdateTenantAcquaintanceReferralCommand>
    {
        private readonly ILogger<CreateTenantAcquaintanceReferralCommandHandler> _logger;
        private readonly IRepository<TenantAcquaintanceReferrals, Guid> _tenantAcquaintanceReferralsRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenantPersonalInformationRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;
        private readonly IEncryptionHandler _encryptionHandler;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        private readonly IOtpHandler _otpHandler;

        public UpdateTenantAcquaintanceReferralCommandHandler(
            IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepository,
            ILogger<CreateTenantAcquaintanceReferralCommandHandler> logger,
            IRepository<TenantBasicInformation, Guid> tenantPersonalInformationRepository,
            IEmailHandler emailHandler,
            IOptions<FrontendSettings> settings,
            IEncryptionHandler encryptionHandler,
            IContextFactory contextFactory,
            IMessageService messageService,
            IOtpHandler otpHandler)
        {
            _logger = logger;
            _tenantAcquaintanceReferralsRepository = tenantAcquaintanceReferralsRepository;
            _tenantPersonalInformationRepository = tenantPersonalInformationRepository;
            _emailHandler = emailHandler;
            _settings = settings;
            _encryptionHandler = encryptionHandler;
            _contextFactory = contextFactory;
            _messageService = messageService;
            _otpHandler = otpHandler;
        }

        public async Task HandleAsync(IContext context, UpdateTenantAcquaintanceReferralCommand command)
        {
            _logger.LogDebug($"Update tenant acquaintance referral for tenant {command.TenantId}");

            var referralId = Guid.NewGuid();

            var actualData = await _tenantAcquaintanceReferralsRepository.GetByIdAsync(command.TenantId);

            if (actualData.Referrals!.Count() < 10)
            {
                var tenant = await _tenantPersonalInformationRepository.GetByIdAsync(command.TenantId);
                var tenantFullName = $"{tenant.FirstName} {tenant.LastName}";

                actualData.Referrals!.Add(new TenantAcquaintanceReferral()
                {
                    Id = referralId,
                    FullName = command.FullName,
                    TenantName = tenantFullName,
                    Email = command.Email,
                    Phone = new Domain.ContactInformationPhoneNumber(command.Phone.Number, command.Phone.CountryCode),
                    Status = "Created",
                    Relationship = command.Relationship,
                    OtherRelationship = command.OtherRelationship,
                    CreatedOn = DateTimeOffset.Now,
                    TenantId = command.TenantId
                }); ;

                await _tenantAcquaintanceReferralsRepository.Update(actualData);

                var tenantUpdatedEvent = new TenantAcquaintanceReferralUpdateEvent(actualData.Id);

                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);

                _logger.LogDebug("Tenant acquaintance referral updated.");

                // Now we have to send the email with the required info

                var subject = "Acquaintance Referral";

                _logger.LogDebug("Tenant: {tenantId} - {fullName}", tenant.Id, tenantFullName);

                var otpId = Guid.NewGuid();
                await _otpHandler.SetOtpProcessAsync(otpId, referralId, command.TenantId, command.Email, command.Email, 43200, "ReferralFeedback", true, false);

                var link = EmailHelper.BuildTenantAcquaintanceReferralFeedbackOtpLink(
                    baseUrl: _settings?.Value?.Url,
                    otpId: otpId);

                _logger.LogDebug("Link: {link}", link);

                var emailBody = EmailHelper.BuildTenantAcquaintanceReferralEmailBody(command.FullName, tenantFullName, link);

                var isSent = await _emailHandler.SendEmailAsync(command.Email, subject, emailBody);

                if (!isSent)
                    _logger.LogError("Not sending email.");

                _logger.LogDebug("Email sended successfully!");
            }
        }
    }
}