using Totira.Bussiness.UserService.Domain;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    public static class ApplicationDetailCompletionExtension
    {
        public static bool ApplicationComplete(this TenantApplicationDetails application)
        {
            return application != null &&
                   !string.IsNullOrWhiteSpace(application.EstimatedRent) &&
                   application.Occupants.Adults > 0;
        }
    }
}
