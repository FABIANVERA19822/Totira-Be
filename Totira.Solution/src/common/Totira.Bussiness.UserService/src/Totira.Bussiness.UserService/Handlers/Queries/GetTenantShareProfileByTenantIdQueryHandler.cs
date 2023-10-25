using System;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
	public class GetTenantShareProfileByTenantIdQueryHandler : IQueryHandler<QueryTenantShareProfileByTenantId, GetTenantShareProfileDto>
	{
        private readonly IRepository<TenantShareProfile, Guid> _tenantShareProfileRepository;
        private readonly ILogger<GetTenantShareProfileByTenantIdQueryHandler> _logger;
        public GetTenantShareProfileByTenantIdQueryHandler(IRepository<TenantShareProfile, Guid> tenantShareProfileRepository,
           ILogger<GetTenantShareProfileByTenantIdQueryHandler> logger)
		{
            _tenantShareProfileRepository = tenantShareProfileRepository;
            _logger = logger;

        }

        public async Task<GetTenantShareProfileDto> HandleAsync(QueryTenantShareProfileByTenantId query)
        {
            var response = new GetTenantShareProfileDto();
            var info = (await _tenantShareProfileRepository.Get(i=>i.TenantId == query.TenantId && i.EncryptedAccessCode == query.EncryptedAccessCode && i.Email == query.Email)).FirstOrDefault();

            if (info == null)
                return new GetTenantShareProfileDto() { TenantId = query.TenantId };

            response.TenantId = query.TenantId;
            response.Email = info.Email;
            response.TypeOfContact = info.TypeOfContact;
            response.PropertyStreetAddress = info.PropertyStreetAddress;
            response.Message = info.Message;
            response.EncryptedAccessCode = info.EncryptedAccessCode;



            return response;


        }
    }
}



