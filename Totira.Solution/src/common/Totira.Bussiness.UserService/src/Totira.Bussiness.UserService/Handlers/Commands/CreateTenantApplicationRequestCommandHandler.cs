using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantApplicationRequestCommandHandler : IMessageHandler<CreateTenantApplicationRequestCommand>
    {
        private readonly ILogger<CreateTenantApplicationRequestCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly IRepository<TenantApplicationDetails, Guid> _tenantApplicationDetailsRepository;

        public CreateTenantApplicationRequestCommandHandler(
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
            IRepository<TenantApplicationDetails, Guid> tenantApplicationDetailsRepository,
            ILogger<CreateTenantApplicationRequestCommandHandler> logger
            )
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _tenantApplicationDetailsRepository = tenantApplicationDetailsRepository;
            _logger = logger;
        }


        /// <summary>
        /// Create a new application id for the tenant
        /// if the command contains latest in true will be attachet to the latest application details
        /// if the command contains latest in false will need the application details id to attach
        /// if the command dont have value for latest will create and empty application request and will be necesary update it with the application details after created
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task HandleAsync(IContext context, CreateTenantApplicationRequestCommand command)
        {
            _logger.LogDebug($"creating tenant application request for tenant id {command.TenantId}");

            Guid? prev = null;
            if (command.ToLatest.HasValue && command.ToLatest.Value)
                prev = (await _tenantApplicationDetailsRepository.Get(ad => ad.TenantId == command.TenantId)).OrderByDescending(d=> d.CreatedOn).FirstOrDefault().Id;

            if (command.ToLatest.HasValue && !command.ToLatest.Value)
                prev = (await _tenantApplicationDetailsRepository.GetByIdAsync(command.ApplicationId.Value)).Id;


            TenantApplicationRequest tenantApplicationRequest = new TenantApplicationRequest()
            {
                Id = Guid.NewGuid(),
                ApplicationDetailsId = prev,
                TenantId = command.TenantId,
                CreatedOn = DateTimeOffset.Now,
            };

            await _tenantApplicationRequestRepository.Add(tenantApplicationRequest);


            var userCreatedEvent = new TenantApplicationRequestCreatedEvent(tenantApplicationRequest.Id);
        }
    }
}
