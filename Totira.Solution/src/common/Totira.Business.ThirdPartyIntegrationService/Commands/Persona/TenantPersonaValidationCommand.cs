using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Business.ThirdPartyIntegrationService.Commands.Persona
{
    [RoutingKey("TenantPersonaValidationCommand")]
    public class TenantPersonaValidationCommand : ICommand
    {
        public Inquiry? data { get; set; }
    }

    public class Inquiry
    {
        public string? type { get; set; }
        public string? id { get; set; }
        public Attributes? attributes { get; set; }
    }

    public class Attributes
    {
        public string? name { get; set; }
        public Payload? payload { get; set; }
        public string? createdat { get; set; }
    }

    public class Payload
    {
        public InquiryData? data { get; set; }
        public List<Included>? included { get; set; }
    }

    public class InquiryData
    {
        public string? type { get; set; }
        public string? id { get; set; }
        public InquiryAttributtes? attributes { get; set; }
        public Relationships? relationships { get; set; }
    }

    public class InquiryAttributtes
    {
        public string? status { get; set; }
        public string? referenceid { get; set; }
        public string? createdat { get; set; }
        public string? completedat { get; set; }
        public string? expiredat { get; set; }
    }

    public class Relationships
    {
        public InquiryReports? reports { get; set; }
        public InquiryTemplate? template { get; set; }
        public InquiryTemplateDetail? inquirytemplate { get; set; }
        public InquiryVerifications? verifications { get; set; }
    }

    public class InquiryReports
    {
        public RelationshipDetail?[] data { get; set; }
    }

    public class InquiryTemplate
    {
        public RelationshipDetail? data { get; set; }
    }

    public class InquiryTemplateDetail
    {
        public RelationshipDetail? data { get; set; }
    }

    public class RelationshipDetail
    {
        public string? id { get; set; }
        public string? type { get; set; }
    }

    public class InquiryVerifications
    {
        public RelationshipDetail[]? data { get; set; }
    }

    public class Included
    {
        public string? type { get; set; }
        public string? id { get; set; }
        public IncludedAttributtes? attributes { get; set; }
    }

    public class IncludedAttributtes
    {
        public string? name { get; set; }
        public string? status { get; set; }
        public string? frontphotourl { get; set; }
        public string? createdat { get; set; }
        public string? completedat { get; set; }
        public string? centerphotourl { get; set; }
        public string? leftphotourl { get; set; }
        public string? rightphotourl { get; set; }
    }

}

