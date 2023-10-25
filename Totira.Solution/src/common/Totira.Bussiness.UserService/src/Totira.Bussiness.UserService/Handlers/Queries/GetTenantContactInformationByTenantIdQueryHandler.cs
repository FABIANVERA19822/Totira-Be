using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.Common;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantContactInformationByTenantIdQueryHandler : IQueryHandler<QueryTenantContactInformationByTenantId, GetTenantContactInformationDto>
    {

        private readonly IRepository<TenantContactInformation, Guid> _tenantAddressInformationRepository;
        private readonly ILogger<GetTenantContactInformationByTenantIdQueryHandler> _logger;

        public GetTenantContactInformationByTenantIdQueryHandler(ILogger<GetTenantContactInformationByTenantIdQueryHandler> logger,
                                                                 IRepository<TenantContactInformation, Guid> tenantAddressInformationRepository)
        {
            _tenantAddressInformationRepository = tenantAddressInformationRepository;
            _logger = logger;
        }
        public async Task<GetTenantContactInformationDto> HandleAsync(QueryTenantContactInformationByTenantId query)
        {
            var response = new GetTenantContactInformationDto();
            Expression<Func<TenantContactInformation, bool>> filter = tai => tai.TenantId == query.TenantId;
            var tenantContactInfo = (await _tenantAddressInformationRepository.Get(filter)).FirstOrDefault();

            if (tenantContactInfo == null)
                return new GetTenantContactInformationDto
                {
                    Id = Guid.NewGuid(),
                    TenantId = query.TenantId,
                    HousingStatus = "",
                    SelectedCountry = "Canada",
                    Province = "",
                    City = "",
                    ZipCode = "",
                    StreetAddress = "",
                    Unit = "",
                    PhoneNumber = new ContactInformationPhoneNumberDto("", ""),
                    Email = ""
                };

            response.HousingStatus = tenantContactInfo.HousingStatus;
            response.SelectedCountry = tenantContactInfo.Country;
            response.Province = tenantContactInfo.Province;
            response.City = tenantContactInfo.City;
            response.ZipCode = tenantContactInfo.ZipCode;
            response.Unit = tenantContactInfo.Unit;
            response.StreetAddress = tenantContactInfo.StreetAddress;
            response.TenantId = tenantContactInfo.TenantId;
            response.Id = tenantContactInfo.Id;
            response.Email = tenantContactInfo.Email;
            response.PhoneNumber = new ContactInformationPhoneNumberDto(tenantContactInfo.PhoneNumber.Number,
                                                                         tenantContactInfo.PhoneNumber.CountryCode);

            return response;
        }
    }
}
