using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public class TenantPropertyApplication: Document, IAuditable, IEquatable<TenantPropertyApplication>
    {
        public string PropertyId { get; set; }
        public Guid ApplicationRequestId { get; set; }
        public Guid ApplicantId { get; set; }
        public string Status { get; set; }

        public Guid MainTenantId;
        public List<Guid> CoApplicantsIds = new List<Guid>();
        public bool IsMulti;

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantPropertyApplication? other)
        {
            throw new NotImplementedException();
        }
    }
}
