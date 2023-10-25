using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.EmailTemplates;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.CommonLibrary.Settings;
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
        private readonly IEmailHandler _emailHandler;
        private readonly IOptions<FrontendSettings> _settings;

        public UpdateTenantApplicationRequestCommandHandler(
           IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
           IRepository<TenantApplicationDetails, Guid> tenantApplicationDetailsRepository,
           IRepository<TenantContactInformation, Guid> tenantContactInfoRepository,
           IRepository<TenantBasicInformation, Guid> tenantBasicInformationRepository,
           IEmailHandler emailHandler,
           ILogger<UpdateTenantApplicationRequestCommandHandler> logger,
           IOptions<FrontendSettings> settings
           )
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _tenantApplicationDetailsRepository = tenantApplicationDetailsRepository;
            _tenantContactInfoRepository = tenantContactInfoRepository;
            _tenantBasicInformationRepository = tenantBasicInformationRepository;
            _emailHandler = emailHandler;
            _logger = logger;
            _settings = settings;
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

            var subject = $"{tenantFullName} invited to join a tenant group application";

            if (command.Coapplicants != null && command.Coapplicants.Any())
            {
                Dictionary<string, Guid?> emailsToSend = new Dictionary<string, Guid?>();
                command.Coapplicants.ForEach(async ca =>
                   {
                       var coAppplicant = new Domain.Coapplicant();
                       var coapplicantId = await ValidateCoaplicantAccount(ca.Email);

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
                               coapplicant = GenerateNewCoapplicant(ca, coApplicants, emailsToSend, coapplicantId);
                           }
                       }
                       else
                       {
                           coapplicant = GenerateNewCoapplicant(ca, coApplicants, emailsToSend, coapplicantId);
                       }
                   }
               );

                foreach (var email in emailsToSend)
                {
                    await SendEmailInvitation(email.Key, tenantFullName, subject, email.Value, applicationRequest.Id, "Co-Applicant");
                }


                applicationRequest.Coapplicants = coApplicants;
            }

            if (command.Guarantor != null)
            {

                Domain.Coapplicant guarantor = new Domain.Coapplicant();

                guarantor.Id = await ValidateCoaplicantAccount(command.Guarantor.Email);
                guarantor.Email = command.Guarantor.Email;
                guarantor.InvitedOn = DateTimeOffset.Now;
                guarantor.Status = (int)TenantCoapplicantStatus.Invited;

                var coapplicantId = await ValidateCoaplicantAccount(guarantor.Email);

                await SendEmailInvitation(guarantor.Email, tenantFullName, subject, coapplicantId, applicationRequest.Id, "Guarantor");

                applicationRequest.Guarantor = guarantor;
            }


            await _tenantApplicationRequestRepository.Update(applicationRequest);


            var userCreatedEvent = new TenantApplicationRequestUpdatedEvent(applicationRequest.Id);


        }

        private static Domain.Coapplicant GenerateNewCoapplicant(UserService.Commands.Coapplicant ca, List<Domain.Coapplicant> coApplicants, Dictionary<string, Guid?> emailsToSend, Guid? coapplicantId)
        {
            Domain.Coapplicant coapplicant = new Domain.Coapplicant() { Id = coapplicantId, Email = ca.Email, InvitedOn = DateTimeOffset.Now, Status = (int)TenantCoapplicantStatus.Invited };
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

        private async Task SendEmailInvitation(string email, string tenantFullName, string subject, Guid? coapplicantId, Guid applicationRequestId, string role)
        {
            var link = GetEmailLink(email, coapplicantId,applicationRequestId);
            var emailBody = EmailHelper.BuildInviteCoapplicantEmailBody(tenantFullName, role, link);
            var isSent = await _emailHandler.SendEmailAsync(email, subject, emailBody);
            if (!isSent)
                _logger.LogError("Not sending email.");

            _logger.LogDebug("Email sended successfully!");
        }

        private string GetEmailLink(string email, Guid? coapplicantId, Guid applicationRequestId)
        {
            string link;
            if (coapplicantId.HasValue)
            {
                _logger.LogDebug("Build body for email invites: {tenantId} - {email}", email);

                link = EmailHelper.BuildInviteCoapplicantLoginLink(
                    baseUrl: _settings.Value.Url,
                    applicationRequestId: applicationRequestId);

                _logger.LogDebug("Link: {link}", link);

            }
            else
            {
                _logger.LogDebug("Build body for email invites: {tenantId} - {email}", email);

                link = EmailHelper.BuildInviteCoapplicantSignUpLink(
                    baseUrl: _settings.Value.Url,
                    applicationRequestId: applicationRequestId);

                _logger.LogDebug("Link: {link}", link);
            }

            return link;
        }

        private async Task<Guid?> ValidateCoaplicantAccount(string email)
        {
            Guid? coApplicantId = null;
            Expression<Func<TenantContactInformation, bool>> expression = (t => t.Email == email);

            var coApplicantInfo = (await _tenantContactInfoRepository.Get(expression)).FirstOrDefault();

            if (coApplicantInfo != null)
                coApplicantId = coApplicantInfo.TenantId;

            return coApplicantId;


        }
    }
}
