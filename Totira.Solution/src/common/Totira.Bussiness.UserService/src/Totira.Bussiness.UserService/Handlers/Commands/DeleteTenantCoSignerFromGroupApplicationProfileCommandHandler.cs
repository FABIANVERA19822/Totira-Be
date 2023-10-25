using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Messages;
using static System.Net.Mime.MediaTypeNames;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
	public class DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler : IMessageHandler<DeleteTenantCoSignerFromGroupApplicationProfileCommand>
    {
        private readonly ILogger<DeleteTenantApplicationRequestGuarantorCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly IRepository<TenantGroupApplicationProfile, Guid> _tenantGroupApplicationProfileRepository;
        public DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler(
             IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
             IRepository<TenantGroupApplicationProfile, Guid> tenantGroupApplicationProfileRepository,
             ILogger<DeleteTenantApplicationRequestGuarantorCommandHandler> logger
            )
		{
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _tenantGroupApplicationProfileRepository = tenantGroupApplicationProfileRepository;
            _logger = logger;
        }

        public async Task HandleAsync(IContext context, DeleteTenantCoSignerFromGroupApplicationProfileCommand command)
        {
            var applicationsRequest = await _tenantApplicationRequestRepository.GetByIdAsync(command.ApplicationRequestId);

            if (applicationsRequest == null)
            {
                _logger.LogError($"The application request {command.ApplicationRequestId} does not exist ");
                return;
            }
            

            if (applicationsRequest?.Guarantor?.Id == command.CoSignerId)
            {
                await DeleteCoSignerFromGroupApplicationProfileInvitationForm(command.MainTenantId, applicationsRequest.Guarantor.Email);


                applicationsRequest.Guarantor = null;
                await _tenantApplicationRequestRepository.Update(applicationsRequest);
            }


            if (applicationsRequest?.Coapplicants != null)
            {
                if (applicationsRequest.Coapplicants.Any(i => i.Id == command.CoSignerId))
                {
                    var coapplicantToBeRemoved = applicationsRequest.Coapplicants.First(ca => ca.Id == command.CoSignerId);

                    await DeleteCoSignerFromGroupApplicationProfileInvitationForm(command.MainTenantId, coapplicantToBeRemoved.Email);

                    applicationsRequest.Coapplicants.Remove(coapplicantToBeRemoved);
                    await _tenantApplicationRequestRepository.Update(applicationsRequest);
                }
            }






        }
        private async Task DeleteCoSignerFromGroupApplicationProfileInvitationForm(Guid tenantId, string email)
        {
            var groupApplicationProfile = (await _tenantGroupApplicationProfileRepository.Get(i => i.TenantId == tenantId && i.Email == email)).FirstOrDefault();

            if (groupApplicationProfile != null)
                await _tenantGroupApplicationProfileRepository.Delete(groupApplicationProfile);
        }

    }
}

