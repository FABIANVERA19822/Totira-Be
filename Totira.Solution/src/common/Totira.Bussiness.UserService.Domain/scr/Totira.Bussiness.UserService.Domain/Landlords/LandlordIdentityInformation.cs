using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Bussiness.UserService.Domain.Landlords
{
    public class LandlordIdentityInformation : Document, IAuditable, IEquatable<LandlordBasicInformation>
    {
        public Guid LandlordId { get; set; }
        public ContactInformationPhoneNumber PhoneNumber { get; set; } = new ContactInformationPhoneNumber(string.Empty, string.Empty);        
        public List<Common.File> IdentityProofs { get; set; }

        #region Audit
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }
        #endregion

        public bool Equals(LandlordBasicInformation? other)
        {
            throw new NotImplementedException();
        }
    }
}
