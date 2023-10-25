
using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public class TenantCurrentJobStatus : Document, IAuditable, IEquatable<TenantCurrentJobStatus>
    {
        public TenantCurrentJobStatus(Guid tenantId, string currentJobStatus, bool isUnderRevisionSend, DateTimeOffset now)
        {
            TenantId = tenantId;
            CurrentJobStatus = currentJobStatus;
            IsUnderRevisionSend = isUnderRevisionSend;
            this.CreatedOn = now;
        }

        public Guid TenantId { get; set; }
        public string CurrentJobStatus { get; set; } = string.Empty;
        public bool IsUnderRevisionSend { get; set; }

        public static TenantCurrentJobStatus CreateCurrentJobStatus(Guid tenantId, string currentJobStatus, bool isUnderRevisionSend)
        => new(tenantId, currentJobStatus, isUnderRevisionSend, DateTimeOffset.Now);
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantCurrentJobStatus? other)
        {
            throw new NotImplementedException();
        }
    }
}
