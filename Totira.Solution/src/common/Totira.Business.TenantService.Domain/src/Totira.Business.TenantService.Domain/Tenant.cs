using Totira.Support.Persistance;
using Totira.Support.Persistance.Entities;

namespace Totira.Business.TenantService.Domain
{
    public class Tenant : Entity, IAuditable, IEquatable<Tenant>
    {
        public string Name { get; set; } = string.Empty;
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(Tenant? other)
        {
            throw new NotImplementedException();
        }
    }
}