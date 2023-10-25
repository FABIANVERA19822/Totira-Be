namespace Totira.Bussiness.UserService.Domain
{
    using Totira.Support.Persistance;
    using Totira.Support.Persistance.Document;

    public class TenantApplicationType : Document, IAuditable, IEquatable<TenantApplicationType>
    {
        public Guid TenantId { get; set; }
        public string ApplicationType { get; set; } = string.Empty;
         
        public Guid CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public bool Equals(TenantApplicationType? other)
        {
            throw new NotImplementedException();
        }
    }
     

}
