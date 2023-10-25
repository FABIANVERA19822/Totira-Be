using static Totira.Support.Application.Messages.IMessageHandler;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;
using Microsoft.Extensions.Logging;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.TransactionalOutbox;
using Totira.Bussiness.UserService.Events;
using LanguageExt;
using Amazon.Runtime.Internal.Util;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class ApplicationRequestInvitationResponseCommandHandler : BaseMessageHandler<ApplicationRequestInvitationResponseCommand, ApplicationRequestInvitationResponseEvent>
    {
        private readonly IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> _invitationsRepository;
        private readonly IRepository<TenantApplicationRequest, Guid> _requestRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _basicInformationRepository;
        public ApplicationRequestInvitationResponseCommandHandler(
             IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> invitationsRepository,
            IRepository<TenantApplicationRequest, Guid> requestRepository,
            IRepository<TenantBasicInformation, Guid> basicInformationRepository,
            ILogger<AcceptTermsAndConditionsCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
            )
            : base(logger, contextFactory, messageService)
        {
            _invitationsRepository = invitationsRepository;
            _requestRepository = requestRepository;
            _basicInformationRepository = basicInformationRepository;
        }

        protected override async  Task<ApplicationRequestInvitationResponseEvent> Process(IContext context, ApplicationRequestInvitationResponseCommand command)
        {
            var invitation = await _invitationsRepository.GetByIdAsync(command.InvitationId);

            if (InvitationIsNotValid(invitation))
            {
                _logger.LogError("The invitation {InvitationId} is not valid.", command.InvitationId);
                return new ApplicationRequestInvitationResponseEvent(Guid.Empty);
            }

            var request = await _requestRepository.GetByIdAsync(invitation.ApplicationRequestId);
            var basicInfo = (await _basicInformationRepository.Get(x => x.TenantEmail == invitation.CoapplicantEmail)).OrderByDescending(x=>x.CreatedOn).FirstOrDefault();

            if (basicInfo is null)
            {
                _logger.LogError("The mail {InvitationCoapplicantEmail} does not belong to a registered user.", invitation.CoapplicantEmail);
                return new ApplicationRequestInvitationResponseEvent(Guid.Empty);
            }

            if (request.Guarantor is not null && request.Guarantor.Email == invitation.CoapplicantEmail)
            {
                if (command.Accepted)
                {
                    request.Guarantor.Id = basicInfo.Id;
                    request.Guarantor.AcceptedOn = DateTimeOffset.UtcNow;
                }
                else
                {
                    request.Guarantor = null;
                }
            }

            if (request.Coapplicants is not null && request.Coapplicants.Any(x => x.Email == invitation.CoapplicantEmail))
            {
                if (command.Accepted)
                {
                    request.Coapplicants.Find(x => x.Email == invitation.CoapplicantEmail).Id = basicInfo.Id;
                    request.Coapplicants.Find(x => x.Email == invitation.CoapplicantEmail).AcceptedOn = DateTimeOffset.UtcNow;

                }
                else
                {
                    request.Coapplicants.Remove(request.Coapplicants.Find(x => x.Email == invitation.CoapplicantEmail));
                }
            }

            await _requestRepository.Update(request);
            await UpdateUsedInvitation(invitation);
            return new ApplicationRequestInvitationResponseEvent(invitation.Id);
        }

        private async Task UpdateUsedInvitation(TenantApplicationRequestCoapplicantsSendEmails invitation)
        {
            invitation.dateTimeExpiration = DateTimeOffset.UtcNow;
            await _invitationsRepository.Update(invitation);
        }

        private bool InvitationIsNotValid(TenantApplicationRequestCoapplicantsSendEmails invitation)
        {
            if (invitation is null ||
                DateTimeOffset.Now > invitation.dateTimeExpiration)
            {
                return true;
            }
            return false;
        }

        
    }
}
