using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Totira.Support.Application.Messages.IMessageHandler;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;
using Microsoft.Extensions.Logging;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.TransactionalOutbox;
using Totira.Bussiness.UserService.Handlers.Queries;
using Totira.Bussiness.UserService.Events;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class ApplicationRequestInvitationResponseCommandHandler : IMessageHandler<ApplicationRequestInvitationResponseCommand>
    {

        private readonly IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> _invitationsRepository;
        private readonly IRepository<TenantApplicationRequest, Guid> _requestRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _basicInformationRepository;
        private readonly ILogger<AcceptTermsAndConditionsCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public ApplicationRequestInvitationResponseCommandHandler(
            IRepository<TenantApplicationRequestCoapplicantsSendEmails, Guid> invitationsRepository,
            IRepository<TenantApplicationRequest, Guid> requestRepository,
            IRepository<TenantBasicInformation, Guid> basicInformationRepository,
            ILogger<AcceptTermsAndConditionsCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService
        )
        {
            _invitationsRepository = invitationsRepository;
            _requestRepository = requestRepository;
            _basicInformationRepository = basicInformationRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }
        public async Task HandleAsync(IContext context, ApplicationRequestInvitationResponseCommand command)
        {
            var invitation = await _invitationsRepository.GetByIdAsync(command.InvitationId);

            if (InvitationIsNotValid(invitation))
            {
                _logger.LogError($"The invitation {command.InvitationId} is not valid.");
                return;
            }

            var request = await _requestRepository.GetByIdAsync(invitation.ApplicationRequestId);
            var basicInfo = (await _basicInformationRepository.Get(x => x.TenantEmail == invitation.CoapplicantEmail)).OrderByDescending(x=>x.CreatedOn).FirstOrDefault();

            if (basicInfo is null)
            {
                _logger.LogError($"The mail {invitation.CoapplicantEmail} does not belong to a registered user.");
                return;
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
            var applicationRequestInvitationResponseCommandEvent = new ApplicationRequestInvitationResponseEvent(invitation.Id);

            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            var messageOutboxId = await _messageService.SendAsync(notificationContext, applicationRequestInvitationResponseCommandEvent);
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
