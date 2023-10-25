namespace Totira.Services.Thirdparty.Worker.Outbox.Bll.EmailTemplates
{
    /// <summary>
    /// Class with useful methods for email process.
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// Build the link for tenant acquaintance referral feedback form link.
        /// </summary>
        /// <param name="baseUrl">Frontend base url.</param>
        /// <param name="tenantId">Tenant Id.</param>
        /// <returns>Redirect link.</returns>
        public static string BuildVerifiedTenantProfileLink(string baseUrl, Guid tenantId)
            => $"{baseUrl}/user";
    }
}
