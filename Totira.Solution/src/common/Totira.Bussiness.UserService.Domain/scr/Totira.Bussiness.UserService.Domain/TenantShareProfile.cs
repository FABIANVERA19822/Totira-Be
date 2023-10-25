using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public class TenantShareProfile : Document, IAuditable, IEquatable<TenantBasicInformation>
    {
        public Guid TenantId { get; set; }

        public string Email { get; set; } = string.Empty;
        public string TypeOfContact { get; set; } = string.Empty;
        public string PropertyStreetAddress { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string EncryptedAccessCode { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool IsAcceptTermsAndConditions { get;  set; } = false;

        public bool Equals(TenantBasicInformation? other)
        {
            throw new NotImplementedException();
        }

        public void AcceptTermsAndConditions()
        {
            this.IsAcceptTermsAndConditions = true;
        }

    }
}

