using Microsoft.Extensions.Options;
using System;
using System.Linq.Expressions;
using System.Net;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO.ThirdpartyService;
using Totira.Bussiness.UserService.Enums;
using Totira.Bussiness.UserService.Extensions;
using Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantProfileFunnelByTenantIdQueryHandler : IQueryHandler<QueryTenantProfileFunnelByTenantId, Dictionary<string, bool>>
    {
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;
        private readonly IRepository<TenantApplicationDetails, Guid> _applicationDetailsRepository;
        private readonly IRepository<TenantBasicInformation, Guid> _basicInfoRepository;
        private readonly IRepository<TenantContactInformation, Guid> _contactInfoRepository;
        private readonly IRepository<TenantEmployeeIncomes, Guid> _employeeIncomesRepository;
        private readonly IRepository<TenantAcquaintanceReferrals, Guid> _tenantAcquaintanceReferralsRepository;
        private readonly IRepository<TenantRentalHistories, Guid> _tenantRentalHistoriesRepository;
        private readonly IRepository<TenantApplicationType, Guid> _tenantApplicationTypeRepository;
        private readonly IRepository<TenantApplicationRequest, Guid> _tenantApplicationRequestRepository;
        private readonly ICommonFunctions _commonFunctions;
        public GetTenantProfileFunnelByTenantIdQueryHandler(IRepository<TenantApplicationDetails, Guid> applicationDetailsRepository,
                                                            IRepository<TenantBasicInformation, Guid> basicInfoRepository,
                                                            IRepository<TenantContactInformation, Guid> contactInfoRepository,
                                                            IRepository<TenantEmployeeIncomes, Guid> employeeIncomesRepository,
                                                            IRepository<TenantRentalHistories, Guid> tenantRentalHistoriesRepository,
                                                            IRepository<TenantAcquaintanceReferrals, Guid> tenantAcquaintanceReferralsRepository,
                                                            IRepository<TenantApplicationType, Guid> tenantApplicationTypeRepository,
                                                            IRepository<TenantApplicationRequest, Guid> tenantApplicationRequestRepository,
                                                            ICommonFunctions commonFunctions,
                                                            IQueryRestClient queryRestClient,
                                                            IOptions<RestClientOptions> restClientOptions
                                                            )
        {
            _applicationDetailsRepository = applicationDetailsRepository;
            _basicInfoRepository = basicInfoRepository;
            _contactInfoRepository = contactInfoRepository;
            _employeeIncomesRepository = employeeIncomesRepository;
            _tenantRentalHistoriesRepository = tenantRentalHistoriesRepository;
            _tenantAcquaintanceReferralsRepository = tenantAcquaintanceReferralsRepository;
            _tenantApplicationTypeRepository= tenantApplicationTypeRepository;
            _tenantApplicationRequestRepository = tenantApplicationRequestRepository;
            _restClientOptions = restClientOptions.Value;
            _queryRestClient = queryRestClient;
            _commonFunctions = commonFunctions;

        }
        public async Task<Dictionary<string, bool>> HandleAsync(QueryTenantProfileFunnelByTenantId query)
        {

            Expression<Func<TenantContactInformation, bool>> contactExpression = x => x.TenantId == query.TenantId;
            Expression<Func<TenantApplicationDetails, bool>> applicationDetailExpression = x => x.TenantId == query.TenantId;
            Expression<Func<TenantApplicationType, bool>> applicationTypeExpression = x => x.TenantId == query.TenantId;
            Expression<Func<TenantApplicationRequest, bool>> cosignersExpression = x => x.TenantId == query.TenantId;

            var application = await (_applicationDetailsRepository.Get(applicationDetailExpression));
            var applicationType = await (_tenantApplicationTypeRepository.Get(applicationTypeExpression));
            var cosigners = await (_tenantApplicationRequestRepository.Get(cosignersExpression));

            var basic = await (_basicInfoRepository.GetByIdAsync(query.TenantId));
            var image = await (_commonFunctions.GetProfilePhoto(new QueryTenantProfileImageById(query.TenantId)));
            var contact = await (_contactInfoRepository.Get(contactExpression));

            var officialID = await GetPersonaValidation(query);

            var employ = await (_employeeIncomesRepository.GetByIdAsync(query.TenantId));
            var history = await (_tenantRentalHistoriesRepository.GetByIdAsync(query.TenantId));
            var referral = await (_tenantAcquaintanceReferralsRepository.GetByIdAsync(query.TenantId));

            Dictionary<string, bool> result = new Dictionary<string, bool>
            {
                { TenantProfileSectionsEnum.Application.GetEnumDescription()   ,application.MaxBy(ap => ap.CreatedOn).ApplicationComplete()},
                { TenantProfileSectionsEnum.ImageProfile.GetEnumDescription()  ,image.ProfileImageComplete()},
                { TenantProfileSectionsEnum.Basic.GetEnumDescription()         ,basic.BasicComplete()},
                { TenantProfileSectionsEnum.Contact.GetEnumDescription()       ,contact.FirstOrDefault().ContactComplete()},
                { TenantProfileSectionsEnum.OfficialID.GetEnumDescription()    ,officialID.OfficialIDComplete()},
                { TenantProfileSectionsEnum.Employment.GetEnumDescription()    ,employ.EmploymentComplete()},
                { TenantProfileSectionsEnum.RentalHistory.GetEnumDescription() ,history.HistoryComplete()},
                { TenantProfileSectionsEnum.Referrals.GetEnumDescription()     ,referral.ReferralComplete()},
                { TenantProfileSectionsEnum.ApplicationType.GetEnumDescription(),applicationType.FirstOrDefault().ApplicationTypeComplete()},
                { TenantProfileSectionsEnum.Cosigners.GetEnumDescription()     ,cosigners.FirstOrDefault().CosignersComplete()}
            };

            return result;
        }
        private async Task<GetPersonaApplicationDto> GetPersonaValidation(QueryTenantProfileFunnelByTenantId query)
        {
            GetPersonaApplicationDto result = new GetPersonaApplicationDto();
            var thirdPartyPersonaResponse = await _queryRestClient.GetAsync<GetPersonaApplicationDto>($"{_restClientOptions.ThirdPartyIntegration}/Persona/applicants/{query.TenantId}");
            if (thirdPartyPersonaResponse.StatusCode is HttpStatusCode.OK)
            {
                result = thirdPartyPersonaResponse.Content;
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
}
