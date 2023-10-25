using Totira.Bussiness.UserService.Domain;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    public static class ApplicationRequestCompletionExtension
    {
        public static bool ApplicationRequestComplete(this TenantApplicationRequest request)
        {
            bool result = false;
            result = request is not null &&
                     request.ApplicationDetailsId is not null;
            return result;
        }
    }
}
