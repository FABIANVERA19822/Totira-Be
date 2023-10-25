
using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("CreateTenantCurrentJobStatusCommand")]
    public class CreateTenantCurrentJobStatusCommand : ICommand
    {
        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }
        public string CurrentJobStatus { get; set; }
        [Required(ErrorMessage = "Under Revision Send is required.")]
        public bool IsUnderRevisionSend { get; set; }
    }
}
