using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain;

public class TenantVerificationProfile : Document, IAuditable, IEquatable<TenantVerificationProfile>
{
    public bool IsVerificationRequested { get; set; }
    public bool IsProfileValidationComplete { get; set; }
    public Guid CreatedBy { get; set;}

    public DateTimeOffset CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public bool Equals(TenantVerificationProfile? other)
    {
        throw new NotImplementedException();
    }
}