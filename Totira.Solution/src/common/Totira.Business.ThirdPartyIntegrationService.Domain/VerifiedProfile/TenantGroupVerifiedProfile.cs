
using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Business.ThirdPartyIntegrationService.Domain.VerifiedProfile
{
    public class TenantGroupVerifiedProfile : Document, IAuditable, IEquatable<TenantGroupVerifiedProfile>
    {
        public TenantGroupVerifiedProfile() { }
        private TenantGroupVerifiedProfile(
        Guid tenantId,
        bool certn,
        bool jira,
        bool persona,
        bool isVerifiedEmailConfirmation,
        DateTimeOffset createdOn)
        {
            TenantId = tenantId;
            Certn = certn;
            Jira = jira;
            Persona = persona;
            IsVerifiedEmailConfirmation = isVerifiedEmailConfirmation;
            CreatedOn = createdOn;
        }
        public Guid TenantId { get; set; }
        public bool Certn { get; set; }
        public bool Jira { get; set; }
        public bool Persona { get; set; }
        public bool IsVerifiedEmailConfirmation { get; set; }

        public static TenantGroupVerifiedProfile CreateGroupVerifiedProfile(Guid TenantId, bool certn, bool jira, bool persona, bool isVerifiedEmailConfirmation)
        => new(TenantId, certn, jira, persona, isVerifiedEmailConfirmation, DateTimeOffset.Now);
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantGroupVerifiedProfile? other)
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

            if (obj is not TenantGroupVerifiedProfile)
                return false;

            return Equals(obj as TenantGroupVerifiedProfile);
        }

        public override int GetHashCode() => Id.GetHashCode() * 42;
    }
}