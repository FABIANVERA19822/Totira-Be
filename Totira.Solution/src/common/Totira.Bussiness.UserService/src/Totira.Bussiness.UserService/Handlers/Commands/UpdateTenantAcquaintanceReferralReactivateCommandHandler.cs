using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.CommonlHandlers;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantAcquaintanceReferralReactivateCommandHandler : IMessageHandler<UpdateTenantAcquaintanceReferralReactivateCommand>
    {
        private readonly ILogger<UpdateTenantAcquaintanceReferralReactivateCommandHandler> _logger;
        private readonly IRepository<TenantAcquaintanceReferrals, Guid> _tenantAcquaintanceReferralsRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenantPersonalInformationRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;
        private readonly IEncryptionHandler _encryptionHandler;
        private readonly IOtpHandler _otpHandler;

        public UpdateTenantAcquaintanceReferralReactivateCommandHandler(
            IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepository,
            ILogger<UpdateTenantAcquaintanceReferralReactivateCommandHandler> logger,
            IRepository<TenantBasicInformation, Guid> tenantPersonalInformationRepository,
            IEmailHandler emailHandler,
            IOptions<FrontendSettings> settings,
            IEncryptionHandler encryptionHandler,
            IOtpHandler otpHandler)
        {
            _logger = logger;
            _tenantAcquaintanceReferralsRepository = tenantAcquaintanceReferralsRepository;
            _tenantPersonalInformationRepository = tenantPersonalInformationRepository;
            _emailHandler = emailHandler;
            _settings = settings;
            _encryptionHandler = encryptionHandler;
            _otpHandler = otpHandler; 
        }


        public async Task HandleAsync(IContext context, UpdateTenantAcquaintanceReferralReactivateCommand command)
        {
            _logger.LogDebug($"Update tenant acquaintance referral reactivating with id {command.ReferralId}");


            Expression<Func<TenantAcquaintanceReferrals, bool>> expression = (r => r.Referrals.Where(l => l.Id == command.ReferralId).Any());


            var actualData = await _tenantAcquaintanceReferralsRepository.Get(expression);

            var referralInfo = actualData.FirstOrDefault();
            var baseReferral = referralInfo.Referrals.Where(r => r.Id == command.ReferralId).FirstOrDefault();



            if (baseReferral != null)
            {
                referralInfo.Referrals.Remove(baseReferral);
                baseReferral.CreatedOn = DateTimeOffset.Now;
                baseReferral.UpdatedOn = DateTimeOffset.Now;
                referralInfo.Referrals.Add(baseReferral);
                await _tenantAcquaintanceReferralsRepository.Update(referralInfo);


                var tenant = await _tenantPersonalInformationRepository.GetByIdAsync(baseReferral.TenantId);
                var subject = "Acquaintance Referral";
                var tenantFullName = $"{tenant.FirstName} {tenant.LastName}";

                _logger.LogDebug("Tenant: {tenantId} - {fullName}", tenant.Id, tenantFullName);

                var otpId = Guid.NewGuid();
                await _otpHandler.SetOtpProcessAsync(otpId, command.ReferralId, baseReferral.TenantId, baseReferral.Email, baseReferral.Email, 43200, "ReferralFeedback", true, false);

                var link = EmailHelper.BuildTenantAcquaintanceReferralFeedbackOtpLink(
                    baseUrl: _settings.Value.Url,
                    otpId: otpId);

                var emailBody = EmailHelper.BuildTenantAcquaintanceReferralEmailBody(baseReferral.FullName, tenantFullName, link);

                var isSent = await _emailHandler.SendEmailAsync(baseReferral.Email, subject, emailBody);

                if (!isSent)
                    _logger.LogError("Not sending email.");

                _logger.LogDebug("Email sended successfully!");
            }
        }
    }
}
