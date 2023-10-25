using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.Persistance;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantPropertyApplicationQueryHandler : IQueryHandler<QueryTenantPropertyApplication, GetPropertyAppliedDto>
    {
        private readonly IRepository<TenantPropertyApplication, Guid> _tenantPropertyApplicationRepository;
        private readonly ILogger<GetTenantPropertyApplicationQueryHandler> _logger;
        public GetTenantPropertyApplicationQueryHandler(
            ILogger<GetTenantPropertyApplicationQueryHandler> logger,
            IRepository<TenantPropertyApplication, Guid> tenantPropertyApplicationRepository
            )
        {
            _tenantPropertyApplicationRepository = tenantPropertyApplicationRepository;
            _logger = logger;
        }
        public async Task<GetPropertyAppliedDto> HandleAsync(QueryTenantPropertyApplication query)
        {
            _logger.LogInformation("Validate existence of property application by applicationrequest");


            Expression<Func<TenantPropertyApplication, bool>> expression = (s => s.ApplicationRequestId == query.ApplicationRequestId && s.PropertyId.Equals(query.PropertyId));
            var propertyapplication = (await _tenantPropertyApplicationRepository.Get(expression)).FirstOrDefault();

            var result =
             propertyapplication != null ?
            new GetPropertyAppliedDto(propertyapplication.PropertyId, propertyapplication.ApplicationRequestId, propertyapplication.ApplicantId, propertyapplication.Id,  propertyapplication.CreatedOn) :
            new GetPropertyAppliedDto();

            return result;
        }
    }
}
