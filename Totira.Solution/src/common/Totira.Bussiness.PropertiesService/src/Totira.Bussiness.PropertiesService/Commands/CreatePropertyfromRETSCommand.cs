namespace Totira.Bussiness.PropertiesService.Commands
{
    using Totira.Support.Application.Commands;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("CreatePropertyfromRETSCommand")]
    public class CreatePropertyfromRETSCommand : ICommand
    {
        public string PropertyType { get; set; } = string.Empty;

    }
}
