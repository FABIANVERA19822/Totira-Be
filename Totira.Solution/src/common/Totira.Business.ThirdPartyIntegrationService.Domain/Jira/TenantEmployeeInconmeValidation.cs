using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Business.ThirdPartyIntegrationService.Domain.Jira
{
    public class TenantEmployeeInconmeValidation : ExternalDocument, IAuditable, IEquatable<TenantEmployeeInconmeValidation>
    {
        public Guid TenantId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string FinalDecision { get; set; }
        public List<string> Comments { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantEmployeeInconmeValidation? other)
        {
            return true;
        }
    }
}
