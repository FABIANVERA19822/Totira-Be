using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class CreateTenantEmploymentReferenceCommandHandler : IMessageHandler<CreateTenantEmploymentReferenceCommand>
    {
        private readonly IRepository<TenantEmploymentReference, Guid> _tenantEmploymentReferenceRepository;
        private readonly ILogger<CreateTenantEmploymentReferenceCommandHandler> _logger;

        public CreateTenantEmploymentReferenceCommandHandler(
           IRepository<TenantEmploymentReference, Guid> tenantEmploymentReferenceRepository,
           ILogger<CreateTenantEmploymentReferenceCommandHandler> logger)
        {
            _tenantEmploymentReferenceRepository = tenantEmploymentReferenceRepository;
            _logger = logger;

        }

        public async Task HandleAsync(IContext context, CreateTenantEmploymentReferenceCommand command)
        {
            _logger.LogDebug($"creating tenant Employment Reference with id {command.TenantId}");

            var tenantEmploymentReference = new TenantEmploymentReference()
            {
                Id = command.TenantId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                JobTitle = command.JobTitle,
                Relationship = command.Relationship,
                Email = command.Email,
                PhoneNumber = new Domain.EmploymentReferencePhoneNumber(command.PhoneNumber.Number, command.PhoneNumber.CountryCode),
                CreatedOn = DateTimeOffset.Now,
            };

            await _tenantEmploymentReferenceRepository.Add(tenantEmploymentReference);
            var userCreatedEvent = new TenantEmploymentReferenceCreatedEvent(tenantEmploymentReference.Id);

        }
    }
}

