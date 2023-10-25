using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Queries;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.PropertiesService.Enums;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantApplicationRoleQueryHandler: IQueryHandler<QueryTenantApplicationRoleByTenantId, string>
    {
        private readonly IRepository<TenantApplicationRequest, Guid> _applicationRequestRepository;
        public GetTenantApplicationRoleQueryHandler(IRepository<TenantApplicationRequest, Guid> applicationRequestRepository) 
        {
            _applicationRequestRepository = applicationRequestRepository;
        }

        public async Task<string> HandleAsync(QueryTenantApplicationRoleByTenantId query)
        {
            string result = null;
            Expression<Func<TenantApplicationRequest,bool>> requestExpression = x=>x.ApplicationDetailsId == query.ApplicationDetailsId;

            var requests = (await _applicationRequestRepository.Get(requestExpression));



            result = GetTennatApplicationRoleFromRequest(requests.FirstOrDefault(), query.TenantId);

            return result;
        }

        private string GetTennatApplicationRoleFromRequest(TenantApplicationRequest request, Guid tenantId)
        {
            if (request == null) 
            { throw new ArgumentNullException(nameof(request)); }

            string result = null;

            if (tenantId == request.TenantId)
            {
                result = TenantApplicationRolesEnum.Main.GetEnumDescription();
            }
            if (request.Coapplicants.All(x=>x.Id == tenantId))
            {
                result = TenantApplicationRolesEnum.Coapplicant.GetEnumDescription();
            }
            if (tenantId == request.Guarantor.Id)
            {
                result = TenantApplicationRolesEnum.Guarantor.GetEnumDescription();
            }

            return result;
        }
    }
}
