using System.Linq.Expressions;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.PropertiesService.Handlers.Queries
{
    public class QueryPropertyByMlnumHandler : IQueryHandler<QueryPropertyByMlnum, GetPropertyDetailsDto>
    {
        private readonly IRepository<Property, string> _propertydataRepository;
        public QueryPropertyByMlnumHandler(IRepository<Property, string> propertydataRepository)

        {
            _propertydataRepository = propertydataRepository;
        }
        public async Task<GetPropertyDetailsDto> HandleAsync(QueryPropertyByMlnum query)
        {

            Expression<Func<Property, bool>> expression = (p => p.Id == query.Ml_num);

            var info = (await _propertydataRepository.Get(expression)).FirstOrDefault();
            

            var result =
                info != null ?
                    new GetPropertyDetailsDto(info.Id,info.Area, info.Address, GetFullWordOfOrientation(info.residential.FrontingOnNSEW),info.ApproxSquareFootage, info.residential.LotDepth, info.residential.LotFront, info.Bedrooms,
                    info.Washrooms, info.ParkingSpaces, info.TotalParkingSpaces, info.residential.GarageSpaces, info.condo.PetsPermitted, info.RemarksForClients, info.AirConditioning,info.condo.Balcony,
                    info.CableTVIncluded, info.Furnished, info.HeatIncluded, info.HydroIncluded, info.KitchensPlus, info.LaundryAccess, info.ParkingIncluded, info.PrivateEntrance, info.residential.Pool,
                    info.condo.BuildingAmenities, info.PropertyFeatures, info.OriginalPrice, info.ListPrice) :
                    new GetPropertyDetailsDto();

            return result;


        }
        public string GetFullWordOfOrientation(string orientation)
        {
            switch (orientation)
            {
                case "N":
                    return "North";
                   
                case "S":
                    return "South";
                case "E":
                    return "East";
                case "W":
                    return "West";

                default:
                    return orientation;
                    
            }
        }
    }
}