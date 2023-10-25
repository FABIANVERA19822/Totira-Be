using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries.Landlord;

public record QueryLandlordBasicInformationById(Guid LandlordId) : IQuery;
