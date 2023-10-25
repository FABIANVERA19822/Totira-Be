using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands
{
    [RoutingKey("UpdateTenantRentalHistoriesReactivateCommand")]
    public class UpdateTenantRentalHistoriesReactivateCommand: ICommand
    {
        public Guid LandlordId { get; set; }
        
    }
}
