using System.Text;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Extensions;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.Otp;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateGroupApplicationShareProfileCommandHandler : IMessageHandler<CreateGroupApplicationShareProfileCommand>
    {
        private readonly ILogger<CreateGroupApplicationShareProfileCommandHandler> _logger;
        private readonly IRepository<TenantGroupApplicationShareProfile, Guid> _tenantGroupApplicationShareProfileRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
        private readonly IRepository<TenantApplicationRequest, Guid> _applicationRequestRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;
        private readonly IEncryptionHandler _encryptionHandler;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        private readonly IOtpHandler _otpHandler;

        public CreateGroupApplicationShareProfileCommandHandler(ILogger<CreateGroupApplicationShareProfileCommandHandler> logger,
            IRepository<TenantGroupApplicationShareProfile, Guid> tenantGroupApplicationShareProfileRepository,
            IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
            IRepository<TenantApplicationRequest, Guid> applicationRequestRepository,
            IOptions<FrontendSettings> settings,
            IEmailHandler emailHandler,
            IEncryptionHandler encryptionHandler,
            IContextFactory contextFactory,
            IMessageService messageService,
            IOtpHandler otpHandler)
        {
            _logger = logger;
            _tenantGroupApplicationShareProfileRepository = tenantGroupApplicationShareProfileRepository;
            _tenantBasicInformationRepository = tenantBasicInformationRepository;
            _applicationRequestRepository = applicationRequestRepository;
            _emailHandler = emailHandler;
            _settings = settings;
            _encryptionHandler = encryptionHandler;
            _contextFactory = contextFactory;
            _messageService = messageService;
            _otpHandler = otpHandler;
        }

        public async Task HandleAsync(IContext context, Either<Exception, CreateGroupApplicationShareProfileCommand> command)
        {
            await command.MatchAsync(async cmd =>
            {
                _logger.LogDebug("creating tenant GroupApplication Share Profile for application request: {ApplicationId}", cmd.ApplicationId);

                var request = await _applicationRequestRepository.GetByIdAsync(cmd.ApplicationId);

                if (request == null)
                {
                    _logger.LogError("the ApplicationId {ApplicationId} not exist in the DB", cmd.ApplicationId);
                    return;
                }

                Random objRandom = new Random();
                int accessCode = objRandom.Next(100000, 999999);
                var otpId = Guid.NewGuid();
                string encryptedAccessCode = _encryptionHandler.EncryptString(accessCode.ToString());

                _logger.LogDebug($"deleting old tenant GroupApplication Share Profile");

                DeleteAllLastShareProfile(cmd.ApplicationId, cmd.Email);

                var coapplicants = new List<CoapplicantShareProfile>();
                var coapplicantsDb = request.Coapplicants ?? new List<Domain.Coapplicant>();
                foreach (var tenant in coapplicantsDb)
                {
                    if (tenant.Id != null)
                    {
                        var tenantGroup = new CoapplicantShareProfile()
                        {
                            Status = tenant.Status,
                            TenantId = tenant.Id.Value,
                            InvinteeType = 1
                        };
                        coapplicants.Add(tenantGroup);
                    }
                }

                var tenantGroupApplicationShareProfile = new TenantGroupApplicationShareProfile()
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = cmd.ApplicationId,
                    TenantId = request.TenantId,
                    Coapplicants = coapplicants,
                    Guarantor = request.Guarantor != null ? new CoapplicantShareProfile()
                    {
                        InvinteeType = 2,
                        Status = request.Guarantor.Status,
                        TenantId = request.Guarantor.Id.Value
                    } : null,
                    Email = cmd.Email,
                    Message = cmd.Message,
                    TypeOfContact = cmd.TypeOfContact,
                    EncryptedAccessCode = encryptedAccessCode,
                    PropertyStreetAddress = cmd.PropertyStreetAddress
                };
                var groupapplicationprofile = coapplicants;
                groupapplicationprofile.Add(new CoapplicantShareProfile() { TenantId = request.TenantId });

                if (tenantGroupApplicationShareProfile.Guarantor != null)
                {
                    groupapplicationprofile.Add(tenantGroupApplicationShareProfile.Guarantor);
                }

                await _tenantGroupApplicationShareProfileRepository.Add(tenantGroupApplicationShareProfile);

                await _otpHandler.SetOtpProcessAsync(otpId, request.TenantId, null, cmd.Email, encryptedAccessCode, "MultiTenant", false, true);

                SendEmail(cmd, accessCode.ToString(), groupapplicationprofile, request.TenantId, otpId);

                var tenantGroupApplicationShareProfileCreatedEvent = new TenantGroupApplicationShareProfileCreatedEvent(tenantGroupApplicationShareProfile.Id);
            }, ex =>
            {
                var tenantGroupApplicationShareProfileCreatedEvent = ex.CreateValidatedEventOf<TenantGroupApplicationShareProfileCreatedEvent>("An error occurred while creating the tenant GroupApplication Share Profile");
                throw ex;
            });
        }

        #region Helper Methods

        private async void DeleteAllLastShareProfile(Guid applicationId, string email)
        {
            var lastShareProfile = await _tenantGroupApplicationShareProfileRepository.Get(x => x.ApplicationId == applicationId && x.Email == email);
            if (lastShareProfile.Any())
            {
                foreach (var item in lastShareProfile)
                {
                    await _tenantGroupApplicationShareProfileRepository.Delete(item);
                }
            }
        }

        private async void SendEmail(CreateGroupApplicationShareProfileCommand command, string accessCode, List<CoapplicantShareProfile> groupapplicationprofile, Guid tenantMainApplicantId, Guid otpId)
        {
            List<TenantBasicInformation> tenantGroup = new List<TenantBasicInformation>();

            StringBuilder groupApplicantsHtml = new StringBuilder();

            foreach (var tenantprofile in groupapplicationprofile)
            {
                var tenantBasicInformation = await _tenantBasicInformationRepository.GetByIdAsync(tenantprofile.TenantId);

                groupApplicantsHtml.Append("<td style='width:32px'></td>");
                groupApplicantsHtml.Append(" <td> <table  cellpadding='0' cellspacing='0' style='width:80px;height:80px;background-color:#0e2c6c;border-radius:50%;color:#fff;text-align:center;vertical-align:middle;margin:0 auto'>");
                groupApplicantsHtml.Append("<tbody style='background:url(https://bucket-totira-publicresources-development.s3.amazonaws.com/icons/verifiedIcon.png) no-repeat;background-size:61px 61px;background-position:150% 210%'>");
                groupApplicantsHtml.Append("<tr><td style='vertical-align:middle;font-size:36px;line-height:36px'>" + tenantBasicInformation.FirstName.Substring(0, 1).ToUpper() + "</td></tr>");
                groupApplicantsHtml.Append("</tbody></table>");
                groupApplicantsHtml.Append("<table style='width:100%'>");
                groupApplicantsHtml.Append("<tr><td style='text-align: center; vertical-align:center;font-size:14px;line-height:28px;color: #0E2C6C; font-size: 18px; font-family: sans-serif; font-weight: 500; word-wrap: break-word'>" + tenantBasicInformation.FirstName + "</td></tr>");
                groupApplicantsHtml.Append("</table></td>");

                tenantGroup.Add(tenantBasicInformation);
            }

            if (tenantGroup is not null)
            {
                string link = EmailHelper.BuildProfileSharingOtpLink(baseUrl: _settings.Value.Url, otpId: otpId);
                var emailBody = EmailHelper.BuildGroupApplicationProfileSharingEmailBody(accessCode, link, command.Message, groupApplicantsHtml.ToString(), command.PropertyStreetAddress);

                string subject = "A group of clients are interested in your property";
                var isSent = await _emailHandler.SendEmailAsync(command.Email, subject, emailBody);

                if (!isSent)
                    _logger.LogError("Fail sending email of GroupApplicationShareProfile.");
            }
        }

        #endregion Helper Methods
    }
}