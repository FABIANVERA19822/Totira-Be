using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Queries;
using Totira.Support.Application.Queries;
using X.PagedList;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.PropertiesService.Handlers.Queries
{
    public class QueryLocationsBySearchKeywordHandler : IQueryHandler<QueryLocationsBySearchKeyword, List<LocationDto>>
    {
        private readonly IRepository<Property, string> _propertydataRepository;
        public QueryLocationsBySearchKeywordHandler(IRepository<Property, string> propertydataRepository)
        {
            _propertydataRepository = propertydataRepository;
        }
        public async Task<List<LocationDto>> HandleAsync(QueryLocationsBySearchKeyword query)
        {
            Expression<Func<Property, bool>> expression =

                (
                p=>
                string.Equals(p.Area, query.SearchKeyword, StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(p.Province, query.SearchKeyword, StringComparison.OrdinalIgnoreCase) ||
                p.Area.ToLower().Contains(query.SearchKeyword.ToLower()) ||
                p.Province.ToLower().Contains(query.SearchKeyword.ToLower())     
                );
                

           List<LocationDto> locations= (await _propertydataRepository.Get(expression)).Select(a =>
                new LocationDto
                {
                    CityName = a.Area,
                    Province_Abbreviation = a.Province,
                }
                ).ToList();

          

            return locations;
        }
    }
}
