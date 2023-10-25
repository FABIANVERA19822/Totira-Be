using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
    public class UpdateTenantEmploymentReferenceCommandHandler : IMessageHandler<UpdateTenantEmploymentReferenceCommand>
    {
        private readonly IRepository<TenantEmploymentReference, Guid> _tenantEmploymentReferenceRepository;
        private readonly ILogger<CreateTenantEmploymentReferenceCommandHandler> _logger;

        public UpdateTenantEmploymentReferenceCommandHandler(
           IRepository<TenantEmploymentReference, Guid> tenantEmploymentReferenceRepository,
           ILogger<CreateTenantEmploymentReferenceCommandHandler> logger)
        {
            _tenantEmploymentReferenceRepository = tenantEmploymentReferenceRepository;
            _logger = logger;

        }

        public async Task HandleAsync(IContext context, UpdateTenantEmploymentReferenceCommand command)
        {
            _logger.LogDebug($"updating tenant Employment Reference with id {command.TenantId}");

            var actualData = await _tenantEmploymentReferenceRepository.GetByIdAsync(command.TenantId);

            actualData.FirstName = actualData.FirstName != command.FirstName ? command.FirstName : actualData.FirstName;
            actualData.LastName = actualData.LastName != command.LastName ? command.LastName : actualData.LastName;
            actualData.JobTitle = actualData.JobTitle != command.JobTitle ? command.JobTitle : actualData.JobTitle;
            actualData.Relationship = actualData.Relationship != command.Relationship ? command.Relationship : actualData.Relationship;
            actualData.Email = actualData.Email != command.Email ? command.Email : actualData.Email;
            actualData.PhoneNumber.Number = actualData.PhoneNumber.Number != command.PhoneNumber.Number ? command.PhoneNumber.Number : actualData.PhoneNumber.Number;
            actualData.PhoneNumber.CountryCode = actualData.PhoneNumber.CountryCode != command.PhoneNumber.CountryCode ? command.PhoneNumber.CountryCode : actualData.PhoneNumber.CountryCode;

            await _tenantEmploymentReferenceRepository.Update(actualData);

            var tenantUpdatedEvent = new TenantEmploymentReferenceUpdatedEvent(actualData.Id);
        }
    }
}

