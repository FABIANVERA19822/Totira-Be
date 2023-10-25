
using Microsoft.Extensions.Logging;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using System.Linq.Expressions;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantCurrentJobStatusByIdQueryHandler : IQueryHandler<QueryCurrentJobStatusByTenantId, GetTenantCurrentJobStatusByDto>
    {
        private readonly ILogger<GetTenantCurrentJobStatusByIdQueryHandler> _logger;
        private readonly IRepository<TenantCurrentJobStatus, Guid> _tenantCurrentJobRepository;

        public GetTenantCurrentJobStatusByIdQueryHandler(
            IRepository<TenantCurrentJobStatus, Guid> tenantCurrentJobStatusRepository,
            ILogger<GetTenantCurrentJobStatusByIdQueryHandler> logger
            )
        {
            _tenantCurrentJobRepository = tenantCurrentJobStatusRepository;
            _logger = logger;
        }

        public async Task<GetTenantCurrentJobStatusByDto> HandleAsync(QueryCurrentJobStatusByTenantId query)
        {
            _logger.LogInformation("Getting Current Job Status from tenantId");
            Expression<Func<TenantCurrentJobStatus, bool>> expression = (tcj => tcj.TenantId == query.Id);
            var info = await _tenantCurrentJobRepository.Get(expression);

            var result =
               info is not null && info.Count() > 0 ?
               new GetTenantCurrentJobStatusByDto() { CurrentJobStatus = info.FirstOrDefault().CurrentJobStatus, 
                   IsUnderRevisionSend = info.FirstOrDefault().IsUnderRevisionSend, TenantId = query.Id } :
               new GetTenantCurrentJobStatusByDto() { CurrentJobStatus = string.Empty, IsUnderRevisionSend = false, TenantId = query.Id };

            return result;
        }
    }
}
