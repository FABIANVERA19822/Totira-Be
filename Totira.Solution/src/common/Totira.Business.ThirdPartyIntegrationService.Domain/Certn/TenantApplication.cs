using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Business.ThirdPartyIntegrationService.Domain.Certn;

public sealed class TenantApplication : ExternalDocument, IAuditable, IEquatable<TenantApplication>
{
    public TenantApplication() { }
    private TenantApplication(
        string id,
        Guid applicantId,
        string statusEquifax,
        string statusSoftCheck,
        string response,        
        DateTimeOffset createdOn)
    {
        Id = id;
        ApplicantId = applicantId;
        StatusEquifax = statusEquifax;
        Response = response;
        StatusSoftCheck = statusSoftCheck;        
        CreatedOn = createdOn;
    }

    public Guid ApplicantId { get; set; }
    public string StatusSoftCheck { get; set; } = default!;
    public string StatusEquifax { get; set; } = default!;
    public string Response { get; set; } = default!;
    

    public static TenantApplication CreateSoftCheck(string applicationId, Guid applicantId, string statusEquifax, string statusSoftCheck, string response)
        => new(applicationId, applicantId, statusEquifax, statusSoftCheck, response, DateTimeOffset.Now);
    public static TenantApplication UpdateSoftCheck(string applicationId, Guid applicantId, string statusEquifax, string statusSoftCheck, string response)
        => new(applicationId, applicantId, statusEquifax, statusSoftCheck, response, DateTimeOffset.Now);
       

    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public bool Equals(TenantApplication? other)
    {
        if (other is null)
            return false;

        if (this.Id != other.Id)
            return false;

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is not TenantApplication)
            return false;

        return Equals(obj as TenantApplication);
    }

    public override int GetHashCode() => Id.GetHashCode() * 42;
}
