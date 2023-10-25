using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;


public class UpdateTenantShareProfileTermsHandler : IMessageHandler<UpdateTenantShareProfileTermsCommand>
{
    private readonly ILogger<UpdateTenantShareProfileTermsHandler> _logger;
    private readonly IRepository<TenantShareProfile, Guid> _tenantContactInfoRepository;
    public UpdateTenantShareProfileTermsHandler(ILogger<UpdateTenantShareProfileTermsHandler> logger,
                                                IRepository<TenantShareProfile, Guid> tenantContactInfoRepository)
    {
        _logger = logger;
        _tenantContactInfoRepository = tenantContactInfoRepository;
    }
    public async Task HandleAsync(IContext context, UpdateTenantShareProfileTermsCommand command)
    {
        _logger.LogDebug($"updating  tenant share profile terms and conditions {command.Id}");

        var tenantShareProfile = (await _tenantContactInfoRepository.Get(item => item.Id == command.Id)).FirstOrDefault();

        if (tenantShareProfile != null)
        {
            tenantShareProfile.AccaptTermsAndConditions();
            await _tenantContactInfoRepository.Update(tenantShareProfile);
            var userCreatedEvent = new TenantEmploymentReferenceCreatedEvent(tenantShareProfile.Id);
        }
    }
}


