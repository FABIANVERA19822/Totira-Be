using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands;

[RoutingKey("UpdateTenantShareProfileTermsCommand")]

public class UpdateTenantShareProfileTermsCommand : ICommand
{
    [Required(ErrorMessage = "Id is required.")]
    public Guid Id { get; set; }
}
