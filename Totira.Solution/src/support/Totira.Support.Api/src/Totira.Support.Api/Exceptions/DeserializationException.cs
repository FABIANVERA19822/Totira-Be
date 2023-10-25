namespace Totira.Support.Api.Exceptions
{
    public class DeserializationException : Exception
    {
        public DeserializationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
