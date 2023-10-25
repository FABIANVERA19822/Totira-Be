using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain;

public class TenantGroupVerificationProfile : Document, IEquatable<TenantGroupVerificationProfile>
{
    public TenantGroupVerificationProfile()
    {
    }

    private TenantGroupVerificationProfile(
        bool isUnderInitialRevision,
        bool isInitialVerificationComplete,
        DateTime initialyVerifiedOn,
        bool isUnderNewRevision,
        bool isLastVerificationComplete,
        DateTime lastVerifiedOn)
    {
        IsUnderInitialRevision = isUnderInitialRevision;
        IsInitialVerificationComplete = isInitialVerificationComplete;
        InitialyVerifiedOn = initialyVerifiedOn;
        IsUnderNewRevision = isUnderNewRevision;
        IsLastVerificationComplete = isLastVerificationComplete;
        LastVerifiedOn = lastVerifiedOn;
    }

    public bool IsUnderInitialRevision { get; set; }
    public bool IsInitialVerificationComplete { get; set; }
    public DateTime InitialyVerifiedOn { get; set; }
    public bool IsReVerificationEnabled { get; set; }
    public bool IsUnderNewRevision { get; set; }
    public bool IsLastVerificationComplete { get; set; }
    public DateTime? LastVerifiedOn { get; set; }

    public bool Equals(TenantGroupVerificationProfile? other)
    {
        throw new NotImplementedException();
    }

    public void InitVerification()
    {
        IsUnderInitialRevision = true;
        IsInitialVerificationComplete = false;
    }

    public void CompleteInitialVerification()
    {
        IsUnderInitialRevision = false;
        IsInitialVerificationComplete = true;
        InitialyVerifiedOn = DateTime.UtcNow;
    }

    public void InitReVerification()
    {
        IsReVerificationEnabled = true;
        IsUnderNewRevision = true;
        IsLastVerificationComplete = false;
    }

    public void CompleteReVerification()
    {
        IsUnderNewRevision = false;
        IsLastVerificationComplete = true;
        LastVerifiedOn = DateTime.UtcNow;
    }
    
    public static TenantGroupVerificationProfile Empty(Guid tenantId) => new() { Id = tenantId };
}