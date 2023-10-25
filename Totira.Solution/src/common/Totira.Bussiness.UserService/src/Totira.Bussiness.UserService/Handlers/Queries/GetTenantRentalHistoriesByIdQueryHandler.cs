using MongoDB.Driver;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantRentalHistoriesByIdQueryHandler : IQueryHandler<QueryTenantRentalHistoriesById, GetTenantRentalHistoriesDto>
    {
        private readonly IRepository<TenantRentalHistories, Guid> _TenantRentalHistoriesRepository;
        public GetTenantRentalHistoriesByIdQueryHandler(IRepository<TenantRentalHistories, Guid> TenantRentalHistoriesRepository)
        {
            _TenantRentalHistoriesRepository = TenantRentalHistoriesRepository;
        }

        public async Task<GetTenantRentalHistoriesDto> HandleAsync(QueryTenantRentalHistoriesById query)
        {
            var info = await _TenantRentalHistoriesRepository.GetByIdAsync(query.Id);

            if (info == null)
                return new GetTenantRentalHistoriesDto() { TenantId = query.Id };

            var rentalHistories = new List<TenantRentalHistoryDto>();

            if (info.RentalHistories != null && info.RentalHistories.Any())
            {
                info.RentalHistories
                    .ForEach(R =>
                    {
                        if (R.CreatedOn.AddMinutes(5) < DateTime.Now && R.Status == "Requested")
                            R.Status = "Expired";

                        rentalHistories
                       .Add(new TenantRentalHistoryDto(
                           R.Id,
                           new DTO.CustomDate(
                               R.RentalStartDate!.Month,
                               R.RentalStartDate!.Year),
                           R.CurrentlyLivingHere,
                           new DTO.CustomDate(
                               R.RentalEndDate!.Month,
                               R.RentalEndDate!.Year),
                           R.Country,
                           R.State,
                           R.City,
                           R.Address,
                           R.Unit,
                           R.ZipCode,
                           R.Status,
                           new DTO.LandlordContactInformation(
                               R.ContactInformation!.Relationship,
                               R.ContactInformation!.FirstName,
                               R.ContactInformation!.LastName,
                               new DTO.RentalHistoriesPhoneNumber(
                                   R.ContactInformation!.PhoneNumber.Number,
                                   R.ContactInformation!.PhoneNumber.CountryCode),
                               R.ContactInformation!.EmailAddress)
                           ));
                    });
            }

            var result = new GetTenantRentalHistoriesDto()
            {
                TenantId = info.Id,
                RentalHistories = rentalHistories
            };

            return result;

        }
    }
}

