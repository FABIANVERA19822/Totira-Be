
using static Totira.Support.Persistance.IRepository;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using Totira.Bussiness.UserService.Commands;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.Profile
{
    public static class CurrentJobExtension
    {
        public static async Task VerifyAndUpdateCurrentJobStatus(this IRepository<TenantCurrentJobStatus, Guid> repositoryCurrentJob, IContextFactory contextFactory, IEventBus bus, Guid TenantId)
        {
            Expression<Func<TenantCurrentJobStatus, bool>> expression = cjs => cjs.TenantId == TenantId;
            var tenantCurrentJobStatus = (await repositoryCurrentJob.Get(expression)).FirstOrDefault();

            if (tenantCurrentJobStatus != null && tenantCurrentJobStatus.IsUnderRevisionSend)
            {

                var currentJob = new UpdateTenantCurrentJobStatusCommand()
                {
                    CurrentJobStatus = tenantCurrentJobStatus.CurrentJobStatus,
                    TenantId = TenantId,
                    IsUnderRevisionSend = false
                };

                var userContext = contextFactory.Create(string.Empty, Guid.Empty);

                await bus.PublishAsync(userContext, currentJob);

                tenantCurrentJobStatus.IsUnderRevisionSend = false;

                await repositoryCurrentJob.Update(tenantCurrentJobStatus);

            }
        }
    }
}
