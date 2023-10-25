namespace Totira.Support.Api.Exceptions
{
    public class TimeoutException : Exception
    {
        public TimeoutException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
