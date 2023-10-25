using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands.ThirdpartyService;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.EventServiceBus;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.Profile
{
    public static class ProfileExtension
    {
        public static async Task VerifyAndUpdateJiraAndVerifiedProfile(this IRepository<TenantApplicationDetails, Guid> repositoryApplicationDetails, IContextFactory contextFactory, IEventBus bus, Guid TenantId)
        {
            Expression<Func<TenantApplicationDetails, bool>> expression = ad => ad.TenantId == TenantId;
            var tenantApplicationDetail = (await repositoryApplicationDetails.Get(expression)).MaxBy(x => x.CreatedOn);

            if (tenantApplicationDetail != null && tenantApplicationDetail.IsVerificationsRequested.HasValue && tenantApplicationDetail.IsVerificationsRequested.Value)
            {

                var profileToUnverify = new UpdateTenantVerifiedProfileCommand()
                {
                    TenantId = TenantId,
                    Jira = false
                };

                var thirdpartyContext = contextFactory.Create(string.Empty, Guid.Empty);

                await bus.PublishAsync(thirdpartyContext, profileToUnverify);

                tenantApplicationDetail.IsVerificationsRequested = false;

                await repositoryApplicationDetails.Update(tenantApplicationDetail);

            }
        }
    }
}
