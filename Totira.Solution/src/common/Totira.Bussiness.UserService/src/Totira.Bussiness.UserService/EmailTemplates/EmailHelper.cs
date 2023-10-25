using Amazon.SimpleEmail.Model;

namespace Totira.Bussiness.UserService.EmailTemplates
{
    /// <summary>
    /// Class with useful methods for email process.
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// Build the email body for tenant acquanintance referral survey email.
        /// </summary>
        /// <param name="referralName">Referral name.</param>
        /// <param name="tenantName">Tenant name.</param>
        /// <param name="link">Link for mail redirect.</param>
        /// <returns>Email body.</returns>
        public static string BuildTenantAcquaintanceReferralEmailBody(string referralName, string tenantName, string link)
            => EmailTemplateResource.ReferralAcquaintanceTemplate
                .Replace("[ContactName]", referralName)
                .Replace("[TenantName]", tenantName)
                .Replace("[Link]", link);

        /// <summary>
        /// Build the link for tenant acquaintance referral feedback form link.
        /// </summary>
        /// <param name="baseUrl">Frontend base url.</param>
        /// <param name="referralName">Referral name.</param>
        /// <param name="tenantName">Tenant</param>
        /// <param name="relationship"></param>
        /// <returns>Redirect link.</returns>
        public static string BuildTenantAcquaintanceReferralFeedbackLink(string baseUrl, Guid referralId, Guid tenantId)
            => $"{baseUrl}/referralFeedback?referralId={referralId}&tenantId={tenantId}";


        /// <summary>
        /// Build the email body coapplicants.
        /// </summary>        
        /// <param name="tenantName">Tenant name.</param>
        /// <param name="link">Link for mail redirect.</param>
        /// <returns>Email body.</returns>
        public static string BuildInviteCoapplicantEmailBody(string tenantName, string role, string link)
            => EmailTemplateResource.InviteCoApplicantTemplate                
                .Replace("[TenantName]", tenantName)
                .Replace("[Role]", role)
                .Replace("[Link]", link);

        /// <summary>
        /// Build the link invite coapplicant for create a new account.
        /// </summary>
        /// <param name="baseUrl">Frontend base url.</param>
        /// <param name="applicationRequestId">application request Id.</param>
        /// <returns>Redirect link.</returns>
        public static string BuildInviteCoapplicantSignUpLink(string baseUrl, Guid applicationRequestId)
            => $"{baseUrl}/register?applicationRequestId={applicationRequestId}";


        /// <summary>
        /// Build the link invite coapplicant with account that already exist.
        /// </summary>
        /// <param name="baseUrl">Frontend base url.</param>
        /// <param name="applicationRequestId">application request id.</param>
        /// <returns>Redirect link.</returns>
        public static string BuildInviteCoapplicantLoginLink(string baseUrl, Guid applicationRequestId)
            => $"{baseUrl}/login?applicationRequestId={applicationRequestId}";

        ///////
        /// <summary>
        /// Build the email body for tenant profile sharing email.
        /// </summary>
        /// <param name="accessCode"></param>
        /// <param name="tenantName"></param>
        /// <param name="link"></param>
        /// <param name="customizeMessage"></param>
        /// <returns></returns>
        public static string BuildProfileSharingEmailBody(string accessCode, string tenantName, string link,string tenantFirstName,string? customizeMessage, string? firstCharFromName , string? propertyStreetAddress)
               => EmailTemplateResource.ProfileSharingTemplate
                         .Replace("[AccessCode]", accessCode)
                         .Replace("[TenantName]", tenantName)
                         .Replace("[Link]", link)
                         .Replace("[customizeMessage]",!string.IsNullOrEmpty(customizeMessage)?$"“{customizeMessage}” -" : null)
                         .Replace("[TenantFirstName]", tenantFirstName)
                         .Replace("[FirstCharFromName]", firstCharFromName)
                         .Replace("[PropertyStreetAddress]", propertyStreetAddress)
            ;

        /// <summary>
        /// Build the link for tenant  profile sharing form link.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="tenantId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string BuildProfileSharingLink(string baseUrl,  Guid tenantId,string email)
         => $"{baseUrl}/shared-profile?tenantId={tenantId}&email={email}";

        /// <summary>
        /// Build the link for tenant acquaintance referral feedback form link.
        /// </summary>
        /// <param name="baseUrl">Frontend base url.</param>
        /// <param name="tenantId">Tenant Id.</param>
        /// <returns>Redirect link.</returns>
        public static string BuildVerifiedTenantProfileLink(string baseUrl, Guid tenantId)
            => $"{baseUrl}/verifiedProfile?tenantId={tenantId}";
    }
}
