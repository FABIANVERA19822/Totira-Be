using System;
using System.ComponentModel.DataAnnotations;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Commands.Jira
{
    [RoutingKey("CreateGroupProfileInterestJiraticketCommand")]
    public class CreateGroupProfileInterestJiraticketCommand : ICommand
    {
        [Required(ErrorMessage = "TenantId is required.")]
        public Guid TenantId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "E-mail is not valid")]
        public string AgentEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "ProfileAccessCode is required.")]
        public int ProfileAccessCode { get; set; }
    }
}

