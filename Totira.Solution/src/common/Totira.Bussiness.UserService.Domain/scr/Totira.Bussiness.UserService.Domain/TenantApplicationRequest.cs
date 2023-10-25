using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public class TenantApplicationRequest : Document, IAuditable, IEquatable<TenantApplicationRequest>
    {
        public Guid TenantId { get; set; }
        public Guid? ApplicationDetailsId { get; set; }
        public List<Coapplicant>? Coapplicants { get; set; }
        public Coapplicant? Guarantor { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantApplicationRequest? other)
        {
            throw new NotImplementedException();
        }
    }

    public class Coapplicant
    {
        public string FirstName { get; set; } = string.Empty;
        public Guid? Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTimeOffset InvitedOn { get; set; }
        public DateTimeOffset AcceptedOn { get; set; }

    }


}
