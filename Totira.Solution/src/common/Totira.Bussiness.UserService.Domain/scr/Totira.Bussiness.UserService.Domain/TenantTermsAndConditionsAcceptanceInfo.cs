using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain;

public class TenantTermsAndConditionsAcceptanceInfo : Document, IAuditable, IEquatable<TenantBasicInformation>
{
    public Guid TenantId { get; set; }
    public DateTime SigningDateTime { get; set; }
    public Guid CreatedBy { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public bool Equals(TenantBasicInformation? other)
    {
        throw new NotImplementedException();
    }
}
