namespace Totira.Support.EventServiceBus.Exceptions
{
    public class ConnectionFailedException : Exception
    {
        public ConnectionFailedException(string message)
            : base(message)
        {
        }

        public ConnectionFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
