using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{ 
    public class QueryTenantPropertyApplication : IQuery
    {
        public string PropertyId { get; set; }
        public Guid ApplicationRequestId { get; set; } 
       
        public QueryTenantPropertyApplication(string propertyId, Guid applicationRequestId)
        {
            PropertyId = propertyId;
            ApplicationRequestId = applicationRequestId;
        } 
    }
}
