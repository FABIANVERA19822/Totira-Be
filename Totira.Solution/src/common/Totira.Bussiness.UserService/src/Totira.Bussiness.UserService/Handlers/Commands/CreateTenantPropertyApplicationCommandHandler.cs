using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO.PropertyService;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Events;
using Totira.Bussiness.UserService.Extensions;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantPropertyApplicationCommandHandler : IMessageHandler<CreatePropertyToApplyCommand>
    {
        private readonly IRepository<TenantPropertyApplication, Guid> _tenantPropertyApplicationRepository;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly ILogger<CreateTenantPropertyApplicationCommandHandler> _logger;
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;

        public CreateTenantPropertyApplicationCommandHandler(
            IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository,
            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
            ILogger<CreateTenantPropertyApplicationCommandHandler> logger,
            IQueryRestClient queryRestClient,
            IOptions<RestClientOptions> restClientOptions,
            IContextFactory contextFactory,
            IMessageService messageService
            )
        {
            _tenantPropertyApplicationRepository = tenantPropertyApplicationRepository;
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _logger = logger;
            _queryRestClient = queryRestClient;
            _restClientOptions = restClientOptions.Value;
            _contextFactory = contextFactory;
            _messageService = messageService;
        }

        public async Task HandleAsync(IContext context, Either<Exception, CreatePropertyToApplyCommand> command)
        {
            await command.MatchAsync(async cmd =>
            {
                _logger.LogInformation("Validate if applicationrequest exists");

                var info = _tenantApplicationRequestRepository.GetByIdAsync(cmd.ApplicationRequestId);

                if (info == null)
                {
                    _logger.LogInformation("Application Request Id doesnt exists");
                    return;
                }

                var propertyData = await _queryRestClient.GetAsync<GetPropertyDetailsDto>($"{_restClientOptions.Properties}/Property/propertydata/{cmd.PropertyId}");

                _logger.LogInformation("Validation existance of PropertyId");

                if (string.IsNullOrEmpty(propertyData.Content.Id))
                {
                    _logger.LogInformation("PropertyId doesnt match with any data");
                    return;
                }

                _logger.LogInformation("creating tenant property application with id {ApplicationRequestId}", cmd.ApplicationRequestId);

                var applicationsRequest = (await _tenantApplicationRequestRepository.Get(ap => ap.TenantId == cmd.ApplicantId || ap.Guarantor.Id == cmd.ApplicantId || ap.Coapplicants.Any(i => i.Id == cmd.ApplicantId))).FirstOrDefault();

                Guid mainTenantId = new Guid();
                List<Guid> coApplicantsIds = new List<Guid>();
                bool isMulti = applicationsRequest?.Guarantor != null ? true : applicationsRequest?.Coapplicants != null && applicationsRequest?.Coapplicants.Count() > 0;

                if (applicationsRequest != null)
                {
                    mainTenantId = applicationsRequest.TenantId;

                    if (applicationsRequest?.Guarantor?.Id != null)
                    {
                        coApplicantsIds.Add(applicationsRequest.Guarantor.Id.Value);
                    }

                    if (applicationsRequest?.Coapplicants != null)
                    {
                        foreach (var item in applicationsRequest.Coapplicants)
                        {
                            if (item?.Id != null)
                            {
                                coApplicantsIds.Add(item.Id.Value);
                            }
                        }
                    }
                }

                var tenantPropertyApplication =
                    new TenantPropertyApplication
                    {
                        Id = Guid.NewGuid(),
                        ApplicantId = cmd.ApplicantId,
                        ApplicationRequestId = cmd.ApplicationRequestId,
                        PropertyId = cmd.PropertyId,
                        Status = TenantPropertyApplicationStatusEnum.UnderRevision.GetEnumDescription(),
                        CreatedOn = DateTimeOffset.UtcNow,

                        MainTenantId = mainTenantId,
                        CoApplicantsIds = coApplicantsIds,
                        IsMulti = isMulti
                    };
                await _tenantPropertyApplicationRepository.Add(tenantPropertyApplication);

                var tenantPropertyApplicationEvent = new TenantPropertyApplicationCreatedEvent(tenantPropertyApplication.Id);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantPropertyApplicationEvent);
            }, async ex =>
            {
                var tenantPropertyApplicationEvent = new TenantPropertyApplicationCreatedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, tenantPropertyApplicationEvent);
                throw ex;
            });
        }
    }
}