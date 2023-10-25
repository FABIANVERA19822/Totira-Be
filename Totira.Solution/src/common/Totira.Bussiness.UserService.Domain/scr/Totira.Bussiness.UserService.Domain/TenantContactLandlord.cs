using System;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;


namespace Totira.Bussiness.UserService.Domain
{
    public class TenantContactLandlord : Document, IAuditable, IEquatable<TenantBasicInformation>
    {
        public Guid? TenantId { get; set; }
        public string PropertyId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ContactInformationPhoneNumber PhoneNumber { get; set; } = new ContactInformationPhoneNumber(string.Empty, string.Empty);
        public string Message { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantBasicInformation? other)
        {
            throw new NotImplementedException();
        }
    }
}

