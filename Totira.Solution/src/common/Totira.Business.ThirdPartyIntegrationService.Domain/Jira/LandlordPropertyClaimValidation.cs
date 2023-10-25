using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Business.ThirdPartyIntegrationService.Domain.Jira
{
    public class LandlordPropertyClaimValidation : ExternalDocument, IAuditable, IEquatable<LandlordPropertyClaimValidation>
    {
        public Guid LandlordId { get; set; }
        public string Status { get; set; } = string.Empty;

        public string Ml_num { get; set; } = string.Empty;
        public string FinalDecision { get; set; } = string.Empty;

        public List<string> Comments { get; set; } = new List<string>();
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(LandlordPropertyClaimValidation? other)
        {
            return true;
        }
    }
}