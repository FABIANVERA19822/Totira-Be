using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantApplicationRequestCommandHandler : IMessageHandler<CreateTenantApplicationRequestCommand>
    {
        private readonly ILogger<CreateTenantApplicationRequestCommandHandler> _logger;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly IRepository<TenantApplicationDetails, Guid> _tenantApplicationDetailsRepository;
        private readonly RestClientOptions _restClientOptions;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public CreateTenantApplicationRequestCommandHandler(
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
            IRepository<TenantApplicationDetails, Guid> tenantApplicationDetailsRepository,
            ILogger<CreateTenantApplicationRequestCommandHandler> logger,
            IOptions<RestClientOptions> restClientOptions,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _tenantApplicationDetailsRepository = tenantApplicationDetailsRepository;
            _logger = logger;
            _restClientOptions = restClientOptions.Value;
            _contextFactory = contextFactory;
            _messageService = messageService;
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
        public async Task HandleAsync(IContext context, Either<Exception, CreateTenantApplicationRequestCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("creating tenant application request for tenant id {TenantId}", cmd.TenantId);

                Guid? prev = null;
                if (cmd.ToLatest.HasValue && cmd.ToLatest.Value)
                    prev = (await _tenantApplicationDetailsRepository.Get(ad => ad.TenantId == cmd.TenantId)).OrderByDescending(d => d.CreatedOn).FirstOrDefault().Id;

                if (cmd.ToLatest.HasValue && !cmd.ToLatest.Value)
                    prev = (await _tenantApplicationDetailsRepository.GetByIdAsync(cmd.ApplicationDetailsId.Value)).Id;

                TenantApplicationRequest tenantApplicationRequest = new TenantApplicationRequest()
                {
                    Id = Guid.NewGuid(),
                    ApplicationDetailsId = prev,
                    TenantId = cmd.TenantId,
                    CreatedOn = DateTimeOffset.Now,
                };

                await _tenantApplicationRequestRepository.Add(tenantApplicationRequest);

                // Create TenantGroupVerifiedProfile
                //await CreateTenantGroupVerifiedProfile(command.TenantId);

                var objectEvent = new TenantApplicationRequestCreatedEvent(tenantApplicationRequest.Id);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
            }, async ex => {
                var objectEvent = new TenantApplicationRequestCreatedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
                throw ex;
            });
        }

        public async Task<string> CreateTenantGroupVerifiedProfile(Guid tenantId)
        {
            try
            {
                HttpClient Client = new HttpClient();
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("TenantId", tenantId.ToString()),
                    new KeyValuePair<string, string>("Certn", "false"),
                    new KeyValuePair<string, string>("Jira", "false"),
                    new KeyValuePair<string, string>("Persona", "false"),
                    new KeyValuePair<string, string>("IsVerifiedEmailConfirmation", "false"),
                });

                var createTenantGroupUrl = $"{_restClientOptions.ThirdPartyIntegration}/VerifiedProfile/groupProfiles";

                using HttpResponseMessage response = await Client.PostAsync(createTenantGroupUrl, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "It wasn't possible. Create Tenant Group Verified Profile");
                throw ex;
            }
        }
    }
}