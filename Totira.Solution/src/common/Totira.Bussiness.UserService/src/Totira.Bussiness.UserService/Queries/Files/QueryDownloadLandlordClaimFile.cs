using Totira.Support.Application.Queries;
namespace Totira.Bussiness.UserService.Queries.Files
{
    public record QueryDownloadLandlordClaimFile(Guid LandlordId,
    string FileName) : IQuery;
}
