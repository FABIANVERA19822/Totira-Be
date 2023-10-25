

namespace Totira.Bussiness.UserService.Queries
{
    using Totira.Bussiness.UserService.DTO;
    using Totira.Support.Application.Queries;
    public class QueryTenantVerifiedProfileById : IQuery
    {
        public QueryTenantVerifiedProfileById(Guid id, GetTenantEmployeeIncomesDto? incomesDto)
        {
            this.Id = id;
            this.IncomesDto = incomesDto;
        }

        public Guid Id { get; set; }
        public GetTenantEmployeeIncomesDto? IncomesDto { get; set; }
    }
}
