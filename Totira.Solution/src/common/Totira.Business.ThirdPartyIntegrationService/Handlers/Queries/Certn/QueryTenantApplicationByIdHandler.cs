using System.Linq.Expressions;
using Totira.Business.ThirdPartyIntegrationService.Domain.Certn;
using Totira.Business.ThirdPartyIntegrationService.DTO;
using Totira.Business.ThirdPartyIntegrationService.Queries.Certn;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Business.ThirdPartyIntegrationService.Handlers.Queries.Certn
{
    public class QueryTenantApplicationByIdHandler : IQueryHandler<QueryTenantApplicationById, TenantApplicationDto>
    {
        private readonly IRepository<TenantApplication, string> _certndataRepository;
        public QueryTenantApplicationByIdHandler(IRepository<TenantApplication, string> certndataRepository)
        {
            _certndataRepository = certndataRepository;
        }
        public async Task<TenantApplicationDto> HandleAsync(QueryTenantApplicationById query)
        {
            Expression<Func<TenantApplication, bool>> expression = (p => p.Id == query.Id);
            var info = (await _certndataRepository.Get(expression)).FirstOrDefault();

            var result =
                info != null ?
                    new TenantApplicationDto(info.Id, info.ApplicantId, info.StatusSoftCheck, info.StatusEquifax ,info.Response, info.CreatedOn) :
                    new TenantApplicationDto();

            return result;
        }
    }
}
