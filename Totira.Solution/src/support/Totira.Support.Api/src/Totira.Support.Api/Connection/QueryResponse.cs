using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Totira.Support.Api.Connection
{
    public class QueryResponse<T>
    {
        

        public QueryResponse(HttpStatusCode statusCode, T content)
        {
            StatusCode = statusCode;
            Content = content;
        }

        public QueryResponse(HttpStatusCode statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public HttpStatusCode StatusCode { get; private set; }
        public string ErrorMessage { get; private set; }
        public T Content { get; private set; }

        public static implicit operator ActionResult<T>(QueryResponse<T> response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content;
            }
            else
            {
                return new StatusCodeResult((int)response.StatusCode);
            }
        }
    }
}