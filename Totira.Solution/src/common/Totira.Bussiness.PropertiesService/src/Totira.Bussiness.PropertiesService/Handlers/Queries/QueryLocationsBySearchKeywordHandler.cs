using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Globalization;
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
            List<LocationDto> result = new List<LocationDto>();
            
            if (query.SearchKeyword.ToLower().Equals("all"))
            {
                Expression<Func<Property, bool>> expression = (p => p.Area.Any());
                result = (await _propertydataRepository.Get(expression)).Select(loc => new LocationDto { CityName = loc.Area, Province_Abbreviation = loc.Province }).ToList();
            }
            else
            {
                Expression<Func<Property, bool>> expression = ( p =>  p.Area.ToLower().Contains(query.SearchKeyword.ToLower()) || 
                                                                      p.Province.ToLower().Contains(query.SearchKeyword.ToLower()));

                result= (await _propertydataRepository.Get(expression)).Select(loc => new LocationDto { CityName = loc.Area, Province_Abbreviation = loc.Province }).ToList();
 
            }

            return result.GroupBy(x => x.CityName).Select(x => x.FirstOrDefault()).ToList(); 
        }
    }
}
