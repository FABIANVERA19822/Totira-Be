using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries.Files
{
    public record QueryDownloadLandlordIdentityFile (Guid LandlordId,
    string FileName) : IQuery;
}
