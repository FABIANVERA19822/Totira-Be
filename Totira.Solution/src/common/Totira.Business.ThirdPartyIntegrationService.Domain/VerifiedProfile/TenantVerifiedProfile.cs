using Totira.Business.ThirdPartyIntegrationService.Domain.Certn;
using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.ThirdPartyIntegrationService.Domain.VerifiedProfile
{
    public sealed class TenantVerifiedProfile : Document, IAuditable, IEquatable<TenantVerifiedProfile>
    {
        public TenantVerifiedProfile() { }
        private TenantVerifiedProfile(
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

        public static TenantVerifiedProfile CreateVerifiedProfile(Guid TenantId, bool certn, bool jira, bool persona, bool isVerifiedEmailConfirmation)
        => new(TenantId, certn, jira, persona, isVerifiedEmailConfirmation, DateTimeOffset.Now);
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantVerifiedProfile? other)
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

            if (obj is not TenantVerifiedProfile)
                return false;

            return Equals(obj as TenantVerifiedProfile);
        }

        public override int GetHashCode() => Id.GetHashCode() * 42;
    }
}
