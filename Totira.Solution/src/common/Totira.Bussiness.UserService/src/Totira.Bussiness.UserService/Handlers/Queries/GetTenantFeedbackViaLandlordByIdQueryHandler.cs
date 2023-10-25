using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Handlers.Commands;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetTenantFeedbackViaLandlordByIdQueryHandler : IQueryHandler<QueryTenantFeedbackViaLandlordById, GetTenantFeedbackViaLandlordDto>
    {
        private readonly IRepository<TenantFeedbackViaLandlord, Guid> _tenantFeedbackViaLandlordRepository;
        private readonly IRepository<TenantRentalHistories, Guid> _tenantRentalHistoriesRepository;
        private readonly ILogger<CreateTenantFeedbackViaLandlordCommandHandler> _logger;
        private readonly ICommonFunctions _commonFunctions;

        public GetTenantFeedbackViaLandlordByIdQueryHandler(
            IRepository<TenantFeedbackViaLandlord, Guid> tenantFeedbackViaLandlordRepository,
            IRepository<TenantRentalHistories, Guid> tenantRentalHistoriesRepository,
            ILogger<CreateTenantFeedbackViaLandlordCommandHandler> logger,
            ICommonFunctions commonFunctions
            )
        {
            _tenantFeedbackViaLandlordRepository = tenantFeedbackViaLandlordRepository;
            _tenantRentalHistoriesRepository = tenantRentalHistoriesRepository;
            _logger = logger;
            _commonFunctions = commonFunctions;
        }

        public async Task<GetTenantFeedbackViaLandlordDto> HandleAsync(QueryTenantFeedbackViaLandlordById query)
        {
            var response = new GetTenantFeedbackViaLandlordDto();
            var historyInfo = await _tenantRentalHistoriesRepository.GetLandlordById(query.Id);
            var historyFormInfo = (await _tenantFeedbackViaLandlordRepository.Get((r => r.LandlordId == query.Id))).FirstOrDefault();

            var startMonth = GetMonth(historyInfo.RentalStartDate.Month);
            var endMonth = !historyInfo.CurrentlyLivingHere  ?  GetMonth(historyInfo.RentalEndDate.Month) : "";



            response.TenantName = historyInfo.FullName;
            var endDate = historyInfo.CurrentlyLivingHere ? "Current" : $"{endMonth}, {historyInfo.RentalEndDate.Year}";
            var startDate = $"{startMonth} , {historyInfo.RentalStartDate.Year}";
            response.RentalPeriod = $"{startDate} - {endDate}";
            response.PropertyAddress = historyInfo.Address;
            var photo = await _commonFunctions.GetProfilePhoto(new QueryTenantProfileImageById(historyInfo.TenantId));

            response.PhotoLink = photo.Filename.FileUrl;


            if (historyFormInfo == null)
            {
                response.Status = historyInfo.CreatedOn.AddMinutes(5) < DateTimeOffset.Now ? "Expired" : "Pending";
            }
            else
            {
                response.Status = "Complete";
                response.Comment = historyFormInfo.Comment;
                response.StarScore = historyFormInfo.Score;

                var day = historyFormInfo.CreatedOn.Day;
                var month = historyFormInfo.CreatedOn.ToString("MMM");
                var year = historyFormInfo.CreatedOn.Year;

                response.DateCompletation = $"{year}, {month} {day}";
            }

            return response;
        }

        private string GetMonth(int month)
        {
            switch (month)
            {
                case 0:
                    return "Jan";
                    break;
                case 1:
                    return "Feb";
                    break;
                case 2:
                    return "Mar";
                    break;
                case 3:
                    return "Apr";
                    break;
                case 4:
                    return "May";
                    break;
                case 5:
                    return "Jun";
                    break;
                case 6:
                    return "Jul";
                    break;
                case 7:
                    return "Aug";
                    break;
                case 8:
                    return "Sep";
                    break;
                case 9:
                    return "Oct";
                    break;
                case 10:
                    return "Nov";
                    break;
                case 11:
                    return "Dec";
                    break;              
                
            }
            return string.Empty;
        }
    }

    public static class LandlordById
    {
        public static async Task<TenantRentalHistory> GetLandlordById(this IRepository<TenantRentalHistories, Guid> tenantRentalHistoryRepositorys, Guid landlordId)
        {

            Expression<Func<TenantRentalHistories, bool>> func = (r => r.RentalHistories.Any(rf => rf.Id == landlordId));
            var data = (await tenantRentalHistoryRepositorys.Get(func)).FirstOrDefault();
            return data.RentalHistories.First(r => r.Id == landlordId);

        }
    }
}

