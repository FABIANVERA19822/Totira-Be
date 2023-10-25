namespace Totira.Services.RootService.Commands
{
    using Totira.Support.Application.Commands;
    using Totira.Support.EventServiceBus.Attributes;

    [RoutingKey("CreateTenantProfileImageCommand")]
    public class CreateTenantProfileImageCommand : ICommand
    {
        public Guid TenantId { get; set; }
        public ProfileImageFile File { get; set; } = default!;

    }

    public class ProfileImageFile
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = default!;
        public byte[] Data { get; set; } = default!;

        public ProfileImageFile(string fileName, string contentType, byte[] data)
        {
            FileName = fileName;
            ContentType = contentType;
            Data = data;
        }

        public ProfileImageFile()
        {
        }
    }

}
