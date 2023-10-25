using System.ComponentModel;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.CommonlHandlers;
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
        private readonly ILogger<UpdateTenantAcquaintanceReferralCommandHandler> _logger;
        private readonly IRepository<TenantAcquaintanceReferrals, Guid> _tenantAcquaintanceReferralsRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        private readonly IOtpHandler _otpHandler;

        public UpdateTenantAcquaintanceReferralCommandHandler(
            IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepository,
            ILogger<UpdateTenantAcquaintanceReferralCommandHandler> logger,
            IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
            IEmailHandler emailHandler,
            IOptions<FrontendSettings> settings,
            IContextFactory contextFactory,
            IMessageService messageService,
            IOtpHandler otpHandler)
        {
            _logger = logger;
            _tenantAcquaintanceReferralsRepository = tenantAcquaintanceReferralsRepository;
            _tenantBasicInformationRepository = tenantBasicInformationRepository;
            _emailHandler = emailHandler;
            _settings = settings;
            _contextFactory = contextFactory;
            _messageService = messageService;
            _otpHandler = otpHandler;
        }

        public async Task HandleAsync(IContext context, Either<Exception, UpdateTenantAcquaintanceReferralCommand> command)
        {
            await command.MatchAsync(async cmd =>
            {
                _logger.LogDebug("Update tenant acquaintance referral for tenant {TenantId}", cmd.TenantId);

                var referralId = Guid.NewGuid();

                var actualData = await _tenantAcquaintanceReferralsRepository.GetByIdAsync(cmd.TenantId);

                if (actualData.Referrals!.Count() < 10)
                {
                    var tenant = await _tenantBasicInformationRepository.GetByIdAsync(cmd.TenantId);
                    var tenantFullName = $"{tenant.FirstName} {tenant.LastName}";

                    actualData.Referrals!.Add(new TenantAcquaintanceReferral()
                    {
                        Id = referralId,
                        FullName = cmd.FullName,
                        TenantName = tenantFullName,
                        Email = cmd.Email,
                        Phone = new ContactInformationPhoneNumber(cmd.Phone.Number, cmd.Phone.CountryCode),
                        Status = "Created",
                        Relationship = cmd.Relationship,
                        OtherRelationship = cmd.OtherRelationship,
                        CreatedOn = DateTimeOffset.Now,
                        TenantId = cmd.TenantId
                    }); ;

                    await _tenantAcquaintanceReferralsRepository.Update(actualData);

                    var tenantUpdatedEvent = new TenantAcquaintanceReferralUpdateEvent(actualData.Id);

                    var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                    var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantUpdatedEvent);

                    _logger.LogDebug("Tenant acquaintance referral updated.");

                    // Now we have to send the email with the required info

                    var subject = "Acquaintance Referral";

                    _logger.LogDebug("Tenant: {TenantId} - {FullName}", tenant.Id, tenantFullName);

                    var otpId = Guid.NewGuid();
                    await _otpHandler.SetOtpProcessAsync(otpId, referralId, cmd.TenantId, cmd.Email, cmd.Email, "ReferralFeedback", true, false);

                    var link = EmailHelper.BuildTenantAcquaintanceReferralFeedbackOtpLink(
                        baseUrl: _settings?.Value?.Url,
                        otpId: otpId);

                    _logger.LogDebug("Link: {link}", link);

                    var emailBody = EmailHelper.BuildTenantAcquaintanceReferralEmailBody(cmd.FullName, tenantFullName, link);

                    var isSent = await _emailHandler.SendEmailAsync(cmd.Email, subject, emailBody);

                    if (!isSent)
                        _logger.LogError("Not sending email.");

                    _logger.LogDebug("Email sended successfully!");
                    return isSent;
                }
                return false;
            }, (Func<Exception, bool>) (ex => throw ex));
            _logger.LogDebug("Email sended successfully!");
        }
    }
}