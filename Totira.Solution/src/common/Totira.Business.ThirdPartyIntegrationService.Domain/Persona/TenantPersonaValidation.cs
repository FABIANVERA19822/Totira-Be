using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Business.ThirdPartyIntegrationService.Domain.Persona
{
    public class TenantPersonaValidation : ExternalDocument, IAuditable, IEquatable<TenantPersonaValidation>
    {
        public Guid TenantId { get; set; }
        public List<string> UrlImages { get; set; }
        public string Status { get; set; } = string.Empty;        
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantPersonaValidation? other)
        {
            return true;
        }
    }
}
