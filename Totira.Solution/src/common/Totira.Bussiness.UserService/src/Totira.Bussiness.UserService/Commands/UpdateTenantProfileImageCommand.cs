

namespace Totira.Bussiness.UserService.Commands
{
    using Totira.Support.Application.Commands;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("UpdateTenantProfileImageCommand")]
    public class UpdateTenantProfileImageCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public ProfileImageFileEmpty? File { get; set; } = default!;

    }

    public class ProfileImageFileEmpty
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[]? Data { get; set; } = default;




    }
}
