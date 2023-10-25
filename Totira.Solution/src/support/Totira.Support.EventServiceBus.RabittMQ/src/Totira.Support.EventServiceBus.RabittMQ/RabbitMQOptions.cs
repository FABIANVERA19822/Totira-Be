namespace Totira.Support.EventServiceBus.RabittMQ
{
    public class RabbitMQOptions
    {        
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SubscriptionClientName { get; set; }
        public string CloudEnv { get; set; }
    }
}
