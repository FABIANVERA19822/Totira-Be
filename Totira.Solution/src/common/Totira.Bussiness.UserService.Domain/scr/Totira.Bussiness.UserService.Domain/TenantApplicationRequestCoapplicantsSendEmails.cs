namespace Totira.Bussiness.UserService.Domain
{
    using Totira.Support.Persistance;
    using Totira.Support.Persistance.Document;

    public class TenantApplicationRequestCoapplicantsSendEmails : Document, IAuditable, IEquatable<TenantBasicInformation>
    {
        public string CoapplicantEmail { get; set; } 
        public Guid ApplicationRequestId { get; set; }
        public DateTimeOffset dateTimeExpiration { get; set; }
        public Guid CreatedBy => throw new NotImplementedException();

        public DateTimeOffset CreatedOn => throw new NotImplementedException();

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantBasicInformation? other)
        {
            throw new NotImplementedException();
        }
    }
}
