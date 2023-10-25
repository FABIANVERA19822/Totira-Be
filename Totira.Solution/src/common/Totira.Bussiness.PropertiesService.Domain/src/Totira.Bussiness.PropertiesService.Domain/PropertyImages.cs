using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;


namespace Totira.Bussiness.PropertiesService.Domain
{
    public class PropertyImages :DocumentMLS, IAuditable, IEquatable<PropertyImages>
    {

        public PropertyImages() {

            Propertyimages = new List<PropertyImage>();
        } 
        public List<PropertyImage>? Propertyimages { get; set; }
        public Guid CreatedBy => throw new NotImplementedException();

        public DateTimeOffset CreatedOn => throw new NotImplementedException();

        public Guid? UpdatedBy => throw new NotImplementedException();

        public DateTimeOffset? UpdatedOn => throw new NotImplementedException();

        public bool Equals(PropertyImages? other)
        {
            throw new NotImplementedException();
        }
    }
}
