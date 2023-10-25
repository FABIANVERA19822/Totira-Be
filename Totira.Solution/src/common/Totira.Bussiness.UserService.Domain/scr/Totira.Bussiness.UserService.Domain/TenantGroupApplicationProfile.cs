using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain
{
    public class TenantGroupApplicationProfile : Document, IAuditable, IEquatable<TenantGroupApplicationProfile>
    {
        public TenantGroupApplicationProfile()
        {
        }

        public TenantGroupApplicationProfile(Guid tenantId, string firstName, string email, int invinteeType, 
            int status, bool isVerifiedEmailConfirmation, DateTimeOffset createdOn)
        {
            TenantId = tenantId;
            FirstName = firstName;
            Email = email;
            InvinteeType = invinteeType;
            Status = status;
            IsVerifiedEmailConfirmation = isVerifiedEmailConfirmation;
            CreatedOn = createdOn;
        }

        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int InvinteeType { get; set; }
        public int Status { get; set; } = 1;
        public bool IsVerifiedEmailConfirmation { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn {get;set; }

        public bool Equals(TenantGroupApplicationProfile? other)
        {
            throw new NotImplementedException();
        }
    }

 
}
