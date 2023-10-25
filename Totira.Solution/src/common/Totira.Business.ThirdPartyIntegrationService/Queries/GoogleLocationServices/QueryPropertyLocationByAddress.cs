using System;
using Totira.Support.Application.Queries;

namespace Totira.Business.ThirdPartyIntegrationService.Queries
{
	public class QueryPropertyLocationByAddress : IQuery
    {
        public string Address { get; }
        public QueryPropertyLocationByAddress(string address)
		{
            Address = address;
        }
	}
}

