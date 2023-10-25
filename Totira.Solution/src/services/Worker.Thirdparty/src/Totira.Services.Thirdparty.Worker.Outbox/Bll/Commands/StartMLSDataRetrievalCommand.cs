using System;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.Thirdparty.Worker.Outbox.Bll.Commands;

[RoutingKey("StartMLSDataRetrievalCommand")]
public class StartMLSDataRetrievalCommand : ICommand
{

    public DateTime StartDate { get; set; }
    public string Action { get; set; }

    public StartMLSDataRetrievalCommand()
	{
        StartDate = DateTime.UtcNow;
        Action = "";
    }

    public StartMLSDataRetrievalCommand(DateTime startDate, string status)
    {
        StartDate = startDate;
        Action = status;
    }
}


