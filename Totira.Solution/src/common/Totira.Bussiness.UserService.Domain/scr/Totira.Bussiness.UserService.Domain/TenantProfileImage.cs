
namespace Totira.Bussiness.UserService.Domain
{
    using Totira.Support.Persistance;
    using Totira.Support.Persistance.Document;

    public class TenantProfileImage : Document, IAuditable, IEquatable<TenantProfileImage>
    {
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public Guid CreatedBy => throw new NotImplementedException();

        public DateTimeOffset CreatedOn => throw new NotImplementedException();

        public Guid? UpdatedBy => throw new NotImplementedException();

        public DateTimeOffset? UpdatedOn => throw new NotImplementedException();

        public bool Equals(TenantProfileImage? other)
        {
            throw new NotImplementedException();
        }
    }
}
