namespace Totira.Bussiness.UserService.Handlers.Queries
{
    using Microsoft.Extensions.Logging;
    using static Totira.Support.Persistance.IRepository;
    using Totira.Bussiness.UserService.Domain; 
    using Totira.Bussiness.UserService.Queries;
    using Totira.Support.Application.Queries;
    using Totira.Bussiness.UserService.DTO;
    using System.Threading.Tasks;
    using System.Linq.Expressions;

    public class GetTenantApplicationTypeByIdQueryHandler : IQueryHandler<QueryApplicationTypeByTenantId, GetTenantApplicationTypeByDto>
    {
        private readonly ILogger<GetTenantApplicationTypeByIdQueryHandler> _logger;
        private readonly IRepository<TenantApplicationType, Guid> _tenantApplicationTypeRepository;

        public GetTenantApplicationTypeByIdQueryHandler(
            IRepository<TenantApplicationType, Guid> tenantApplicationTypeRepository,
            ILogger<GetTenantApplicationTypeByIdQueryHandler> logger
            )
        {
            _tenantApplicationTypeRepository = tenantApplicationTypeRepository;
            _logger = logger;
        }

        public async Task<GetTenantApplicationTypeByDto> HandleAsync(QueryApplicationTypeByTenantId query)
        {
            _logger.LogInformation("Getting ApplicationType from tenantId");
            Expression<Func<TenantApplicationType, bool>> expression = (tap => tap.TenantId == query.Id);
            var info = await _tenantApplicationTypeRepository.Get(expression);

            var result =
               info is not null && info.Count()>0 ?
               new GetTenantApplicationTypeByDto() { ApplicationType=  info.FirstOrDefault().ApplicationType, TenantId= query.Id} :
               new GetTenantApplicationTypeByDto() { ApplicationType = string.Empty, TenantId = query.Id };

            return result;
        }

     
    }
}
