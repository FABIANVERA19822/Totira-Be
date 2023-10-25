using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.PropertiesService.Queries
{
    public class QueryLocationsBySearchKeyword : IQuery
    {

        public QueryLocationsBySearchKeyword(string _searchKeyword)
        {
            SearchKeyword = _searchKeyword;
        }
        public string SearchKeyword { get; set; }

    }
}
