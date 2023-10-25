namespace Totira.Bussiness.PropertiesService.Queries
{
    using Totira.Support.Application.Queries;

    public class QueryPropertyByMlnum : IQuery
    {
        public QueryPropertyByMlnum(string ml_num)
        {
          Ml_num = ml_num;
        }
        public string Ml_num { get; }
    }
}
