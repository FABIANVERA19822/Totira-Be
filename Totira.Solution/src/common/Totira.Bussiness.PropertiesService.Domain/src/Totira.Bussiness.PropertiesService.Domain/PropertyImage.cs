using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Persistance.Document;
using Totira.Support.Persistance;

namespace Totira.Bussiness.PropertiesService.Domain
{
    public class PropertyImage : Document, IAuditable, IEquatable<PropertyImage>
    {
        public string S3KeyName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        
        public int ImageOrder { get; set; }
       
        public Guid CreatedBy => throw new NotImplementedException();

        public DateTimeOffset CreatedOn => throw new NotImplementedException();

        public Guid? UpdatedBy => throw new NotImplementedException();

        public DateTimeOffset? UpdatedOn => throw new NotImplementedException();

        public bool Equals(PropertyImage? other)
        {
            throw new NotImplementedException();
        }
    }
}
