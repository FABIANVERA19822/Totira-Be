using Totira.Support.Application.Events;
using Totira.Support.EventServiceBus.Attributes;
using Totira.Support.NotificationHub;

namespace Totira.Services.RootService.Events.UserService;

[RoutingKey("TenantEmployeeIncomeFileDeletedEvent")]
public class TenantEmployeeIncomeFileDeletedEvent : IEvent, INotification
{
    public Guid TenantId { get; set; }
    public Guid IncomeId { get; set; }
    public string FileName { get; set; }
    
    public TenantEmployeeIncomeFileDeletedEvent(Guid tenantId, Guid incomeId,string fileName) {
        TenantId = tenantId;
        IncomeId = incomeId;
        FileName = fileName;
    }
    public NotificationMessage GetNotificationMessage()
    {
        var info = new TenantEmployeeIncomeFileDeletedInfo(this.TenantId,this.IncomeId,this.FileName);
        var json = System.Text.Json.JsonSerializer.Serialize(info);
        return new NotificationMessage(NotificationMessageStatus.Success, json);
    }

    private class TenantEmployeeIncomeFileDeletedInfo
    {
        public Guid TenantId { get; set; }
        public Guid IncomeId { get; set; }
        public string FileName { get; set; }

        public TenantEmployeeIncomeFileDeletedInfo(Guid tenantId, Guid incomeId, string fileName)
        {
            this.TenantId = tenantId;
            this.IncomeId = incomeId;
            this.FileName = fileName;

        }
    }
}
