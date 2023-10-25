using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public class TenantFeedbackViaLandlord : Document, IAuditable, IEquatable<TenantBasicInformation>
    {
        public Guid TenantId { get; set; }
        public Guid LandlordId { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; } = string.Empty;
        public Guid CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantBasicInformation? other)
        {
            throw new NotImplementedException();
        }
    }
}
