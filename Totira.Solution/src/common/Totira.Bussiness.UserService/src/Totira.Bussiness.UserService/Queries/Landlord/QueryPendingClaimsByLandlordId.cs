using Totira.Support.Application.Queries;
namespace Totira.Bussiness.UserService.Queries.Landlord;
public record QueryPendingClaimsByLandlordId(Guid LandlordId) : IQuery;