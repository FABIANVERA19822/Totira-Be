
namespace Totira.Bussiness.UserService.Commands
{
    using Totira.Support.Application.Commands;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("CreateTenantProfileImageCommand")]
    public class CreateTenantProfileImageCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public ProfileImageFile File { get; set; } = new();

    }

    public class ProfileImageFile
    {
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public byte[] Data { get; set; } = default!;


    }
}
