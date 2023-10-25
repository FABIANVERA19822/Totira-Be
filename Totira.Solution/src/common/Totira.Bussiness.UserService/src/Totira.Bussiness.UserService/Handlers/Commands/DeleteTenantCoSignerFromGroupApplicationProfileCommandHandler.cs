using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler : IMessageHandler<DeleteTenantCoSignerFromGroupApplicationProfileCommand>
    {
        private readonly ILogger<DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
    
        public DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler(
             IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
             
             ILogger<DeleteTenantCoSignerFromGroupApplicationProfileCommandHandler> logger
            )
		{
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
           
            _logger = logger;
        }

        public async Task HandleAsync(IContext context, Either<Exception, DeleteTenantCoSignerFromGroupApplicationProfileCommand> command)
        {
            await command.MatchAsync(async cmd => {
                var applicationsRequest = await _tenantApplicationRequestRepository.GetByIdAsync(cmd.ApplicationRequestId);

                if (applicationsRequest == null)
                {
                    _logger.LogError("The application request {ApplicationRequestId} does not exist ", cmd.ApplicationRequestId);
                    return;
                }

                if (applicationsRequest?.Guarantor?.Id == cmd.CoSignerId)
                {
                    applicationsRequest.Guarantor = null;
                    await _tenantApplicationRequestRepository.Update(applicationsRequest);
                }

                if (applicationsRequest?.Coapplicants != null)
                {
                    if (applicationsRequest.Coapplicants.Any(i => i.Id == cmd.CoSignerId))
                    {
                        var coapplicantToBeRemoved = applicationsRequest.Coapplicants.First(ca => ca.Id == cmd.CoSignerId);
                        applicationsRequest.Coapplicants.Remove(coapplicantToBeRemoved);
                        await _tenantApplicationRequestRepository.Update(applicationsRequest);
                    }
                }
            }, ex => throw ex);
        }
    }
}
