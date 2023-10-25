using Totira.Support.Application.Queries;

namespace Totira.Bussiness.PropertiesService.Queries
{
    public class QueryPropertyCarrouselImagesByMl_num : IQuery
    {
        public QueryPropertyCarrouselImagesByMl_num(string ml_num)
        {
            Ml_num = ml_num;
        }
        public string Ml_num { get; }
    }
}
