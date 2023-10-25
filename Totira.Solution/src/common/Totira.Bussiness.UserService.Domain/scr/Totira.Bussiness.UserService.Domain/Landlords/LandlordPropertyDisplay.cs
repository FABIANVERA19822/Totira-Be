using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain.Landlords
{
    public class LandlordPropertyDisplay : Document, IAuditable, IEquatable<LandlordBasicInformation>
    {

        public Guid LandlordId { get; set; }
        public string MlsId { get; set; }
        public string Location { get; set; }
        public string Size { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Price { get; set; }
        public string AvaillableDate { get; set; }
        public string Status { get; set; }

        #region Audit
        public Guid CreatedBy => throw new NotImplementedException();

        public DateTimeOffset CreatedOn => DateTimeOffset.Now;

        public Guid? UpdatedBy => throw new NotImplementedException();

        public DateTimeOffset? UpdatedOn => throw new NotImplementedException();
        #endregion

        public bool Equals(LandlordBasicInformation? other)
        {
            throw new NotImplementedException();
        }
    }
}
