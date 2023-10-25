using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Support.Otp.Domain
{
    public class TenantOtpProcess : Document, IAuditable
    {
        public Guid EntityId { get; set; }

        public Guid? EntityKey { get; set; }

        public string Email { get; set; } = string.Empty;

        public int AmountUse { get; set; } = 0;

        public int Retries { get; set; } = 0;

        public string Scope { get; set; } = string.Empty;

        public string EncryptedAccessCode { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset ExpirationDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }
        public bool IsOtpValid { get; set; } = true;
    }
}