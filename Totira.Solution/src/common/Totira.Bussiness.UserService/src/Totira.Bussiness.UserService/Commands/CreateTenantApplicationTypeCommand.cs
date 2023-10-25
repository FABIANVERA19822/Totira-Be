namespace Totira.Bussiness.UserService.Commands
{
    using Totira.Support.Application.Commands;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("CreateTenantApplicationTypeCommand")]
    public class CreateTenantApplicationTypeCommand : ICommand
    {
        public Guid TenantId { get; set; }

        public string ApplicationType { get; set; } = string.Empty;
    }

}
