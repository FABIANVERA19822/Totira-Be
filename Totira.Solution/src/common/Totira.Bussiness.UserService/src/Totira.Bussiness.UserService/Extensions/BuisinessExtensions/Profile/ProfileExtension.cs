using System.Linq.Expressions;
using Totira.Bussiness.UserService.Commands.ThirdpartyService;
using Totira.Bussiness.UserService.Domain;
using Totira.Support.Application.Messages;
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

        public static async Task RequestVerification(
            this IRepository<TenantVerificationProfile, Guid> repository,
            Guid tenantId)
        {
            var tenantVerificationProfile = await repository.GetByIdAsync(tenantId);
            if (tenantVerificationProfile is null)
            {
                tenantVerificationProfile = new();
                await repository.Add(tenantVerificationProfile);
            }
            
            tenantVerificationProfile.IsVerificationRequested = true;
            tenantVerificationProfile.UpdatedOn = DateTime.UtcNow;

            await repository.Update(tenantVerificationProfile);
        }

        public static async Task UnverifyTenant(
            this IRepository<TenantVerificationProfile, Guid> repository,
            Guid tenantId,
            IContextFactory contextFactory,
            IEventBus bus)
        {
            var tenantVerificationProfile = await repository.GetByIdAsync(tenantId);
            if (tenantVerificationProfile is null)
            {
                tenantVerificationProfile = new() { Id = tenantId };
                await repository.Add(tenantVerificationProfile);
            }

            var unverifyCommand = new UpdateTenantVerifiedProfileCommand()
            {
                TenantId = tenantId,
                Jira = false
            };

            var context = contextFactory.Create(string.Empty, Guid.Empty);
            await  bus.PublishAsync(context, unverifyCommand);
            tenantVerificationProfile.IsVerificationRequested = false;
            await repository.Update(tenantVerificationProfile);
        }

        public static async Task CompleteTenantVerification(
            this IRepository<TenantVerificationProfile, Guid> repository,
            Guid tenantId)
        {
            
            var tenantVerificationProfile = await repository.GetByIdAsync(tenantId);
            if (tenantVerificationProfile is null)
            {
                tenantVerificationProfile = new();
                await repository.Add(tenantVerificationProfile);
            }

            tenantVerificationProfile.IsProfileValidationComplete = true;
            tenantVerificationProfile.IsVerificationRequested = false;
            tenantVerificationProfile.UpdatedOn = DateTime.UtcNow;

            await repository.Update(tenantVerificationProfile);
        }
    }
}