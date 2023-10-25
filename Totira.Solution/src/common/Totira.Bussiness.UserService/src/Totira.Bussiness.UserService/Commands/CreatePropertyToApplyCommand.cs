using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("CreatePropertyToApplyCommand")]
    public class CreatePropertyToApplyCommand : ICommand
    {
        [Required(ErrorMessage = "PropertyId is required.")]
        public string PropertyId { get; set; }

        [Required(ErrorMessage = "ApplicationRequestId is required.")]
        public Guid ApplicationRequestId { get; set; }

        [Required(ErrorMessage = "ApplicantId is required.")]
        public Guid ApplicantId { get; set; }
    }
}
