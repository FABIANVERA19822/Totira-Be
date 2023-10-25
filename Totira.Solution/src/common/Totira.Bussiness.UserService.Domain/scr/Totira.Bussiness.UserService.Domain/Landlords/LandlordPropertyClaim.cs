using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain.Landlords
{
    public class LandlordPropertyClaim : Document, IAuditable, IEquatable<LandlordBasicInformation>
    {
        public Guid LandlordId { get; set; }
        public Guid? PropertyId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; }
        public bool HasJiraTicket { get; set; } = false;
        public string Role { get; set; }
        public string MlsID { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Unit { get; set; }
        public string ListingUrl { get; set; }
        public List<Common.File> OwnershipProofs { get; set; }

        #region Audit

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        #endregion Audit

        public bool Equals(LandlordBasicInformation? other)
        {
            throw new NotImplementedException();
        }
    }
}