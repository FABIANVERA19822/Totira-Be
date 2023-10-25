using Totira.Support.Persistance.Document;
using Totira.Support.Persistance;

namespace Totira.Business.ThirdPartyIntegrationService.Domain.Certn;

public sealed class TenantApplications : ExternalDocument, IAuditable, IEquatable<TenantApplications>
{
    public TenantApplications() { }
    private TenantApplications(string id) => Id = id;
    private TenantApplications(string id, TenantApplication item, DateTimeOffset createdOn)
    {
        Id = id;
        Applications.Add(item);
        CreatedOn = createdOn;
    }
    private TenantApplications(string id, ICollection<TenantApplication> items, DateTimeOffset createdOn)
    {
        Id = id;
        Applications.AddRange(items);
        CreatedOn = createdOn;
    }

    public TenantApplications(string id, List<TenantApplication> applications, DateTimeOffset createdOn)
    {
        Id = id;
        Applications = applications;
        CreatedOn = createdOn;
    }

    public List<TenantApplication> Applications { get; set; } = new();

    public static TenantApplications Empty(string tenantId) => new(tenantId);
    public static TenantApplications CreateWithOneApplication(string tenantId, TenantApplication item)
        => new(tenantId, item, DateTimeOffset.Now);
    public static TenantApplications CreateWithListOfApplications(string tenantId, ICollection<TenantApplication> items)
        => new(tenantId, items, DateTimeOffset.Now);

    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }

    public bool Equals(TenantApplications? other)
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

        if (obj is not TenantApplications)
            return false;

        return Equals(obj as TenantApplications);
    }

    public override int GetHashCode() => Id.GetHashCode() * 42;
}
