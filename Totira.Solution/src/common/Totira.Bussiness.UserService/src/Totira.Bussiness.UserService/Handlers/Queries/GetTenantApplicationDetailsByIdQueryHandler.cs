using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.DTO.ThirdpartyService;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Api.Connection;
using Totira.Support.Api.Options;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;
using ApplicationDetailOccupants = Totira.Bussiness.UserService.DTO.ApplicationDetailOccupants;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantApplicationDetailsByIdQueryHandler : IQueryHandler<QueryTenantApplicationDetailsById, GetTenantApplicationDetailsDto>
    {
        private readonly IRepository<TenantApplicationDetails, Guid> _tenatApplicationDetailsRepository;
        private readonly IQueryRestClient _queryRestClient;
        private readonly RestClientOptions _restClientOptions;       

        public GetTenantApplicationDetailsByIdQueryHandler
           (
           IRepository<TenantApplicationDetails, Guid> tenatApplicationDetailsRepository,
           IQueryRestClient queryRestClient,
           IOptions<RestClientOptions> restClientOptions
           )
        {
            _restClientOptions = restClientOptions.Value;
            _tenatApplicationDetailsRepository = tenatApplicationDetailsRepository;
            _queryRestClient = queryRestClient;
        }

        public async Task<GetTenantApplicationDetailsDto> HandleAsync(QueryTenantApplicationDetailsById query)
        {
            Expression<Func<TenantApplicationDetails, bool>> expression = (tap => tap.TenantId == query.Id);
            var existing = await _tenatApplicationDetailsRepository.Get(expression);

            if (!existing.Any())
                return new GetTenantApplicationDetailsDto() { Id = query.Id, Occupants = new ApplicationDetailOccupants(1, 0) };

            var info = existing.MaxBy(apt => apt.CreatedOn);

            var cars = new List<DTO.ApplicationDetailCar>();
            var pets = new List<DTO.ApplicationDetailPet>();

            if (info.Cars != null && info.Cars.Any())
                info.Cars.ForEach(c => cars.Add(new DTO.ApplicationDetailCar(c.Plate, c.Year, c.Make, c.Model)));

            if (info.Pets != null && info.Pets.Any())
                info.Pets.ForEach(p => pets.Add(new DTO.ApplicationDetailPet(p.Size, p.Description, p.Type)));

            var validationResult = await _queryRestClient.GetAsync<GetTenantVerifiedProfileDto>($"{_restClientOptions.ThirdPartyIntegration}/VerifiedProfile/profiles/{query.Id}");

            var IsProfileValidationComplete = false;

            if (validationResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                IsProfileValidationComplete = validationResult.Content.Persona && validationResult.Content.Certn && validationResult.Content.Jira;
            }

            var result = new GetTenantApplicationDetailsDto()
            {
                Id = info.Id,
                Car = cars.Any(),
                Pet = pets.Any(),
                Smoker = info.Smoker,
                EstimatedMove = info.EstimatedMove != null ? new DTO.ApplicationDetailEstimatedMove((info.EstimatedMove.Month+1), info.EstimatedMove.Year) : null,
                EstimatedRent = info.EstimatedRent,
                CarsNumber = cars.Count(),
                PetsNumber = pets.Count(),
                Cars = cars.Any() ? cars : new List<DTO.ApplicationDetailCar>(),
                Pets = pets.Any() ? pets : new List<DTO.ApplicationDetailPet>(),
                Occupants = new DTO.ApplicationDetailOccupants(info.Occupants.Adults, info.Occupants.Childrens),
                IsVerificationsRequested = info.IsVerificationsRequested.HasValue ? info.IsVerificationsRequested.Value : false,
                IsProfileValidationComplete = IsProfileValidationComplete

            };

            return result;
        }
    }
}
