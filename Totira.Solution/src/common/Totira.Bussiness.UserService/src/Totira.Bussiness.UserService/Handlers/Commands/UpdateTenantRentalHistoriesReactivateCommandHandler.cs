using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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
    public class UpdateTenantRentalHistoriesReactivateCommandHandler : IMessageHandler<UpdateTenantRentalHistoriesReactivateCommand>
    {
        private readonly ILogger<UpdateTenantRentalHistoriesReactivateCommandHandler> _logger;
        private readonly IRepository<TenantRentalHistories, Guid> _tenantRentalHistoriesRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenantPersonalInformationRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;
        private readonly IOtpHandler _otpHandler;

        public UpdateTenantRentalHistoriesReactivateCommandHandler(
            IRepository<TenantRentalHistories, Guid> tenantRentalHistoriesRepository,
            ILogger<UpdateTenantRentalHistoriesReactivateCommandHandler> logger,
            IRepository<TenantBasicInformation, Guid> tenantPersonalInformationRepository,
            IEmailHandler emailHandler,
            IOptions<FrontendSettings> settings,
            IOtpHandler otpHandler)
        {
            _logger = logger;
            _tenantRentalHistoriesRepository = tenantRentalHistoriesRepository;
            _tenantPersonalInformationRepository = tenantPersonalInformationRepository;
            _emailHandler = emailHandler;
            _settings = settings;
            _otpHandler = otpHandler;
        }

        public async Task HandleAsync(IContext context, UpdateTenantRentalHistoriesReactivateCommand command)
        {
            _logger.LogDebug($"Update tenant rental history reactivating  for landlord with Id {command.LandlordId}");

            Expression<Func<TenantRentalHistories, bool>> expression = (p => p.RentalHistories.Where(q => q.Id == command.LandlordId).Any());

            var actualData = await _tenantRentalHistoriesRepository.Get(expression);

            var rentalHistoriesInfo = actualData.FirstOrDefault();
            var baseRentalHistories = rentalHistoriesInfo.RentalHistories.Where(p => p.Id == command.LandlordId).FirstOrDefault();

            if (baseRentalHistories != null)
            {
                rentalHistoriesInfo.RentalHistories.Remove(baseRentalHistories);
                baseRentalHistories.CreatedOn = DateTimeOffset.Now;
                baseRentalHistories.UpdatedOn = DateTimeOffset.Now;
                baseRentalHistories.IsFeedbackRequest = true;
                baseRentalHistories.Status = "Requested";

                rentalHistoriesInfo.RentalHistories.Add(baseRentalHistories);
                await _tenantRentalHistoriesRepository.Update(rentalHistoriesInfo);

                var tenant = await _tenantPersonalInformationRepository.GetByIdAsync(baseRentalHistories.TenantId);
                var subject = "Request for Tenant Feedback";
                var tenantFullName = $"{tenant.FirstName} {tenant.LastName}";

                _logger.LogDebug("Tenant: {tenantId} - {fullName}", tenant.Id, tenantFullName);

                string htmlEmail = EmailTemplateResource.ReferralLandlordTemplate;
                var landlordName = $"{baseRentalHistories.ContactInformation.FirstName} {baseRentalHistories.ContactInformation.LastName}";
                var contactInformation = string.Empty;
                var landLordId = baseRentalHistories.Id;
                var otpId = Guid.NewGuid();
                await _otpHandler.SetOtpProcessAsync(otpId, landLordId, baseRentalHistories.TenantId, baseRentalHistories.ContactInformation.EmailAddress, baseRentalHistories.ContactInformation.EmailAddress, 43200, "Feedback", true, false);

                var link = EmailHelper.BuildReferralLandlordfeedbackOtpLink(_settings.Value.Url, otpId);
                htmlEmail = htmlEmail
                    .Replace("[landlordName]", landlordName)
                    .Replace("[tenantName]", tenantFullName)
                    .Replace("[link]", link)
                    .Replace("[contactInformation]", contactInformation);

                var isSent = await _emailHandler.SendEmailAsync(baseRentalHistories.ContactInformation.EmailAddress, subject, htmlEmail);

                if (!isSent)
                    _logger.LogError("Not sending email.");

                _logger.LogDebug("Email sended successfully!");
            }
        }
    }
}