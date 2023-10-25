namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    using Totira.Bussiness.UserService.Domain;
    using static System.Net.Mime.MediaTypeNames;

    public static class CosignersCompletionExtension
    {
        public static bool CosignersComplete(this TenantApplicationRequest applicationRequest)
        {
            return applicationRequest != null &&
                   applicationRequest.Coapplicants != null &&
                   applicationRequest.Coapplicants.Any();
        }
    }
}
