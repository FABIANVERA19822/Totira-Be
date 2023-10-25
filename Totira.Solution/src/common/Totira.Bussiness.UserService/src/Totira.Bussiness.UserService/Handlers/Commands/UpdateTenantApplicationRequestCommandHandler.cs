using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Bussiness.UserService.Enums;
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
    public class UpdateTenantApplicationRequestCommandHandler : IMessageHandler<UpdateTenantApplicationRequestCommand>
    {
        private readonly ILogger<UpdateTenantApplicationRequestCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly IRepository<TenantApplicationDetails, Guid> _tenantApplicationDetailsRepository;
        private readonly IRepository<TenantContactInformation, Guid> _tenantContactInfoRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _tenantBasicInformationRepository;
        private readonly IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> _tenantApplicationRequestCoapplicantsSendEmailsRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;

        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        private readonly IOtpHandler _otpHandler;

        public UpdateTenantApplicationRequestCommandHandler(
           IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
           IRepository<TenantApplicationDetails, Guid> tenantApplicationDetailsRepository,
           IRepository<TenantContactInformation, Guid> tenantContactInfoRepository,
           IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
           IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> tenantApplicationRequestCoapplicantsSendEmailsRepository,
           IEmailHandler emailHandler,
           ILogger<UpdateTenantApplicationRequestCommandHandler> logger,
           IOptions<FrontendSettings> settings,
           IContextFactory contextFactory,
           IMessageService messageService,
           IOtpHandler otpHandler
           )
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _tenantApplicationDetailsRepository = tenantApplicationDetailsRepository;
            _tenantContactInfoRepository = tenantContactInfoRepository;
            _tenantBasicInformationRepository = tenantBasicInformationRepository;
            _tenantApplicationRequestCoapplicantsSendEmailsRepository = tenantApplicationRequestCoapplicantsSendEmailsRepository;
            _emailHandler = emailHandler;
            _logger = logger;
            _settings = settings;
            _contextFactory = contextFactory;
            _messageService = messageService;
            _otpHandler = otpHandler;
        }

        /// <summary>
        /// Update a new application request for the tenant
        /// if the command contains latest in true will be attachet to the latest application details
        /// if the command contains latest in false will need the application details id to attach
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(IContext context, UpdateTenantApplicationRequestCommand command)
        {
            _logger.LogDebug($"creating tenant application request for tenant id {command.TenantId}");

            TenantApplicationDetails applicationDetails = null;
            TenantApplicationRequest applicationRequest = null;

            if (command.ToLatest.HasValue && command.ToLatest.Value)
            {
                Expression<Func<TenantApplicationRequest, bool>> expression = (tar => tar.TenantId == command.TenantId);
                var existing = await _tenantApplicationRequestRepository.Get(expression);
                if (existing != null && existing.Any())
                {
                    applicationRequest = existing.MaxBy(tar => tar.CreatedOn);
                }
                else
                {
                    _logger.LogError($"Tenant dont have any application request");
                    return;
                }
            }
            else
            {
                applicationRequest = await _tenantApplicationRequestRepository.GetByIdAsync(command.ApplicationId);
                if (applicationRequest == null)
                {
                    _logger.LogError($"Application Request {command.ApplicationId} dont exist");
                    return;
                }
            }

            var tenant = await _tenantBasicInformationRepository.GetByIdAsync(command.TenantId);

            if (tenant == null)
            {
                _logger.LogError($"Tenant {command.TenantId} dont exist");
                return;
            }

            var tenantFullName = $"{tenant.FirstName} {tenant.LastName}";
            applicationDetails = await ValidateApplicationDetails(command, applicationDetails);

            if (applicationDetails == null)
            {
                _logger.LogError($"Currently dont exist and application detail for the tenant  {command.TenantId}");
                return;
            }

            applicationRequest.ApplicationDetailsId = applicationRequest.ApplicationDetailsId == null ? applicationDetails.Id : applicationRequest.ApplicationDetailsId;

            List<Domain.Coapplicant> coApplicants = new List<Domain.Coapplicant>();

            var subject = $"{tenant.FirstName} invited to join a Group Application Profile in Totira";

            if (command.Coapplicants != null && command.Coapplicants.Any())
            {
                Dictionary<string, Guid?> emailsToSend = new Dictionary<string, Guid?>();
                command.Coapplicants.ForEach(ca =>
                {
                    Guid? coApplicantId = null;
                    Domain.Coapplicant coapplicant = null;

                    if (applicationRequest.Coapplicants != null)
                    {
                        if (applicationRequest.Coapplicants.Any(co => co.Email == ca.Email))
                        {
                            coapplicant = (applicationRequest.Coapplicants.First(co => co.Email == ca.Email));
                            coApplicants.Add(coapplicant);
                        }
                        else
                        {
                            coapplicant = GenerateNewCoapplicant(ca, coApplicants, emailsToSend, coApplicantId);
                        }
                    }
                    else
                    {
                        coapplicant = GenerateNewCoapplicant(ca, coApplicants, emailsToSend, coApplicantId);
                    }
                }
                );

                foreach (var email in emailsToSend)
                {
                    var coapplicantData = coApplicants.Where(x => x.Email == email.Key).FirstOrDefault();

                    await _tenantApplicationRequestCoapplicantsSendEmailsRepository.Add(new TenantApplicationRequestCoapplicantsSendEmails()
                    {
                        ApplicationRequestId = applicationRequest.Id,
                        CoapplicantEmail = email.Key,
                        dateTimeExpiration = DateTimeOffset.UtcNow.AddDays(30),

                        UpdatedOn = DateTimeOffset.UtcNow,
                        Id = Guid.NewGuid()
                    });

                    await SendEmailInvitation(email.Key, coapplicantData.FirstName, tenantFullName, subject, email.Value, applicationRequest.Id, "Co-Applicant");
                }
            }

            applicationRequest.Coapplicants = coApplicants;

            if (applicationRequest.Guarantor is null || applicationRequest.Guarantor.Id is null)
            {
                await AddNewGuarantor(command, applicationRequest, tenantFullName, subject);
            }

            await _tenantApplicationRequestRepository.Update(applicationRequest);

            var objectEvent = new TenantApplicationRequestUpdatedEvent(applicationRequest.Id);

            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
        }

        private async Task AddNewGuarantor(UpdateTenantApplicationRequestCommand command, TenantApplicationRequest applicationRequest, string tenantFullName, string subject)
        {
            Domain.Coapplicant? guarantor = null;
            if (command.Guarantor != null)
            {
                guarantor = new Domain.Coapplicant();

                guarantor.Id = null;
                guarantor.FirstName = command.Guarantor.FirstName;
                guarantor.Email = command.Guarantor.Email;
                guarantor.InvitedOn = DateTimeOffset.Now;
                guarantor.Status = (int)TenantCoapplicantStatus.Invited;

                if (applicationRequest.Guarantor != null)
                {
                    if (applicationRequest.Guarantor.Email != command.Guarantor.Email)
                    {
                        await StoreSendMailInvitation(applicationRequest.Id, guarantor, tenantFullName, subject);
                    }
                }
                else
                {
                    await StoreSendMailInvitation(applicationRequest.Id, guarantor, tenantFullName, subject);
                }
            }
            applicationRequest.Guarantor = guarantor;
        }

        private static Domain.Coapplicant GenerateNewCoapplicant(UserService.Commands.Coapplicant ca, List<Domain.Coapplicant> coApplicants, Dictionary<string, Guid?> emailsToSend, Guid? coapplicantId)
        {
            Domain.Coapplicant coapplicant = new Domain.Coapplicant() { Id = coapplicantId, Email = ca.Email, FirstName = ca.FirstName, InvitedOn = DateTimeOffset.Now, Status = (int)TenantCoapplicantStatus.Invited };
            coApplicants.Add(coapplicant);
            emailsToSend.Add(ca.Email, coapplicantId);
            return coapplicant;
        }

        private async Task<TenantApplicationDetails> ValidateApplicationDetails(UpdateTenantApplicationRequestCommand command, TenantApplicationDetails applicationDetails)
        {
            if (command.ToLatest.Value)
            {
                applicationDetails = (await _tenantApplicationDetailsRepository.Get(ad => ad.TenantId == command.TenantId)).OrderByDescending(d => d.CreatedOn).FirstOrDefault();
            }
            else
            {
                applicationDetails = (await _tenantApplicationDetailsRepository.GetByIdAsync(command.ApplicationDetailsId.Value));
            }

            return applicationDetails;
        }

        private async Task SendEmailInvitation(string email, string cosignerName, string tenantFullName, string subject, Guid? coapplicantId, Guid applicationRequestId, string role)
        {
            var otpId = Guid.NewGuid();

            var links = await GetEmailLink(email, coapplicantId, applicationRequestId, otpId);

            //await SetOtpProcessAsync(otpId, applicationRequestId,Guid.Parse(links[1]), email);
            await _otpHandler.SetOtpProcessAsync(otpId, applicationRequestId, Guid.Parse(links[1]), email, email, 43200, "Invite", true, false);
            var emailBody = EmailHelper.BuildInviteCoapplicantEmailBody(cosignerName, tenantFullName, role, links[0]);
            var isSent = await _emailHandler.SendEmailAsync(email, subject, emailBody);
            if (!isSent)
                _logger.LogError("Not sending email.");

            _logger.LogDebug("Email sended successfully!");
        }

        private async Task<List<string>> GetEmailLink(string email, Guid? coapplicantId, Guid applicationRequestId, Guid otpId)
        {
            //string link;

            Expression<Func<TenantApplicationRequestCoapplicantsSendEmails, bool>> expressionmail = (tar => tar.ApplicationRequestId == applicationRequestId);
            var existing = (await _tenantApplicationRequestCoapplicantsSendEmailsRepository.Get(expressionmail)).ToList().Where(c => c.CoapplicantEmail == email).FirstOrDefault();

            _logger.LogDebug("Build body for email invites: {applicationRequestId} - {email} - {invitationId}", applicationRequestId, email, existing!.Id);

            //link = $"{_settings.Value.Url}/invite?invitationId={existing.Id}";

            var link = EmailHelper.BuildInviteOtpLink(_settings.Value.Url, otpId);

            _logger.LogDebug("Link: {link}", link);

            var response = new List<string> { link, existing.Id.ToString() };

            return response;
        }

        private async Task StoreSendMailInvitation(Guid applicationRequestId, Domain.Coapplicant guarantor, string tenantFullName, string subject)
        {
            await _tenantApplicationRequestCoapplicantsSendEmailsRepository.Add(new TenantApplicationRequestCoapplicantsSendEmails()
            {
                ApplicationRequestId = applicationRequestId,
                CoapplicantEmail = guarantor.Email,
                dateTimeExpiration = DateTimeOffset.UtcNow.AddDays(30),
                UpdatedOn = DateTimeOffset.UtcNow,
                Id = Guid.NewGuid()
            });

            await SendEmailInvitation(guarantor.Email, guarantor.FirstName, tenantFullName, subject, guarantor.Id, applicationRequestId, "Guarantor");
        }
    }
}