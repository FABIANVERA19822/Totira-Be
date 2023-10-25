﻿
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.PropertiesService.Commands
{
    [RoutingKey("ImportPropertyImagesToS3Command")]
    public class ImportPropertyImagesToS3Command : ICommand
    {
        public ImportPropertyImagesToS3Command(string propertyId)
        {
            PropertyId = propertyId;
        }
        public string PropertyId { get; set; }

     
    }
}
