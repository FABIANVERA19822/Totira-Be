using Totira.Support.Persistance.Entities;

namespace Totira.Support.Persistance
{
    public class AuditableEntity : Entity, IAuditable
    {
        public Guid CreatedBy { get; protected set; }
        public DateTimeOffset CreatedOn { get; protected set; }
        public Guid? UpdatedBy { get; protected set; }
        public DateTimeOffset? UpdatedOn { get; protected set; }
    }
}
