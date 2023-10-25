using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public class TenantGroupApplicationShareProfile : Document, IAuditable, IEquatable<TenantGroupApplicationShareProfile>
    {
        public Guid ApplicationId { get; set; }
        public Guid TenantId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string TypeOfContact { get; set; } = string.Empty;
        public string PropertyStreetAddress { get; set; } = string.Empty; 
        public string EncryptedAccessCode { get; set; } = string.Empty;
        public bool ContactAcceptedTermsAndConditions { get; set; } = false;

        public List<CoapplicantShareProfile>? Coapplicants { get; set; } = new List<CoapplicantShareProfile>();
        public CoapplicantShareProfile? Guarantor { get; set; } = default!;
        public Guid CreatedBy => throw new NotImplementedException();

        public DateTimeOffset CreatedOn => throw new NotImplementedException();

        public Guid? UpdatedBy => throw new NotImplementedException();

        public DateTimeOffset? UpdatedOn => throw new NotImplementedException();

        public bool Equals(TenantGroupApplicationShareProfile? other)
        {
            throw new NotImplementedException();
        }
        public void AcceptTermsAndConditions()
        {
            this.ContactAcceptedTermsAndConditions = true;
        }
    }

    public class CoapplicantShareProfile
    {
        public Guid TenantId { get; set; } 
        public int InvinteeType { get; set; } 
        public int Status { get; set; }
    }
}
