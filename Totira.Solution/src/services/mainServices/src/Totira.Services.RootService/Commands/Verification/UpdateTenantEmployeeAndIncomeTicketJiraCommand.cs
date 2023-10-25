using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands.Verification
{
    [RoutingKey("UpdateTenantEmployeeAndIncomeTicketJiraCommand")]
    public class UpdateTenantEmployeeAndIncomeTicketJiraCommand : ICommand
    {
        public long? timestamp { get; set; }
        public string? webhookEvent { get; set; }
        public string? Issue_event_type_name { get; set; }
        public User? User { get; set; }
        public Comment? Comment { get; set; }
        public Issue? Issue { get; set; }
        public Changelog? changelog { get; set; }
    }
    public class Comment
    {
        public string? self { get; set; }
        public string? id { get; set; }
        public Author? author { get; set; }
        public string? body { get; set; }
        public Updateauthor? updateAuthor { get; set; }
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }
        public bool? jsdPublic { get; set; }
    }

    public class Updateauthor
    {
        public string? self { get; set; }
        public string? accountId { get; set; }
        public Avatarurls1? avatarUrls { get; set; }
        public string? displayName { get; set; }
        public bool? active { get; set; }
        public string? timeZone { get; set; }
        public string? accountType { get; set; }
    }


    public class Author
    {
        public string? self { get; set; }
        public string? accountId { get; set; }
        public Avatarurls? avatarUrls { get; set; }
        public string? displayName { get; set; }
        public bool? active { get; set; }
        public string? timeZone { get; set; }
        public string? accountType { get; set; }
    }


    public class User
    {
        public string? self { get; set; }
        public string? accountId { get; set; }
        public Avatarurls? avatarUrls { get; set; }
        public string? displayName { get; set; }
        public bool? active { get; set; }
        public string? timeZone { get; set; }
        public string? accountType { get; set; }
    }

    public class Avatarurls
    {
        public string? _48x48 { get; set; }
        public string? _24x24 { get; set; }
        public string? _16x16 { get; set; }
        public string? _32x32 { get; set; }
    }

    public class Issue
    {
        public string? id { get; set; }
        public string? self { get; set; }
        public string? key { get; set; }
        public Fields? fields { get; set; }
    }

    public class Fields
    {
        public DateTime? statuscategorychangedate { get; set; }
        public Issuetype? Issuetype { get; set; }
        public object? timespent { get; set; }
        public object? customfield_10030 { get; set; }
        public Project? project { get; set; }
        public object? customfield_10031 { get; set; }
        public object[]? fixVersions { get; set; }
        public object? aggregatetimespent { get; set; }
        public Customfield_10035? customfield_10035 { get; set; }
        public object? resolution { get; set; }
        public object? customfield_10027 { get; set; }
        public object? customfield_10028 { get; set; }
        public object? customfield_10029 { get; set; }
        public object? resolutiondate { get; set; }
        public int? workratio { get; set; }
        public DateTime? lastViewed { get; set; }
        public Watches? watches { get; set; }
        public Issuerestriction? Issuerestriction { get; set; }
        public DateTime? created { get; set; }
        public object? customfield_10020 { get; set; }
        public object? customfield_10021 { get; set; }
        public object? customfield_10022 { get; set; }
        public Priority? priority { get; set; }
        public object? customfield_10023 { get; set; }
        public object? customfield_10024 { get; set; }
        public object? customfield_10025 { get; set; }
        public object[]? labels { get; set; }
        public object? customfield_10026 { get; set; }
        public object? customfield_10016 { get; set; }
        public object? customfield_10017 { get; set; }
        public Customfield_10018? customfield_10018 { get; set; }
        public string? customfield_10019 { get; set; }
        public object? timeestimate { get; set; }
        public object? aggregatetimeoriginalestimate { get; set; }
        public object[]? versions { get; set; }
        public object[]? Issuelinks { get; set; }
        public object? assignee { get; set; }
        public DateTime? updated { get; set; }
        public Status? status { get; set; }
        public object[]? components { get; set; }
        public object? timeoriginalestimate { get; set; }
        public string? description { get; set; }
        public object? customfield_10010 { get; set; }
        public object? customfield_10014 { get; set; }
        public object? customfield_10015 { get; set; }
        public Timetracking? timetracking { get; set; }
        public object? customfield_10005 { get; set; }
        public object? customfield_10006 { get; set; }
        public object? security { get; set; }
        public object? customfield_10007 { get; set; }
        public object? customfield_10008 { get; set; }
        public object[]? attachment { get; set; }
        public object? customfield_10009 { get; set; }
        public object? aggregatetimeestimate { get; set; }
        public string? summary { get; set; }
        public Creator? creator { get; set; }
        public object[]? subtasks { get; set; }
        public Reporter? reporter { get; set; }
        public Aggregateprogress? aggregateprogress { get; set; }
        public object? customfield_10001 { get; set; }
        public object? customfield_10002 { get; set; }
        public object? customfield_10003 { get; set; }
        public object? customfield_10004 { get; set; }
        public object? environment { get; set; }
        public object? duedate { get; set; }
        public Progress? progress { get; set; }
        public Votes? votes { get; set; }
    }

    public class Issuetype
    {
        public string? self { get; set; }
        public string? id { get; set; }
        public string? description { get; set; }
        public string? iconUrl { get; set; }
        public string? name { get; set; }
        public bool? subtask { get; set; }
        public int? avatarId { get; set; }
        public string? entityId { get; set; }
        public int? hierarchyLevel { get; set; }
    }

    public class Project
    {
        public string? self { get; set; }
        public string? id { get; set; }
        public string? key { get; set; }
        public string? name { get; set; }
        public string? projectTypeKey { get; set; }
        public bool? simplified { get; set; }
        public Avatarurls1? avatarUrls { get; set; }
    }

    public class Avatarurls1
    {
        public string? _48x48 { get; set; }
        public string? _24x24 { get; set; }
        public string? _16x16 { get; set; }
        public string? _32x32 { get; set; }
    }

    public class Customfield_10035
    {
        public string? self { get; set; }
        public string? value { get; set; }
        public string? id { get; set; }
    }

    public class Watches
    {
        public string? self { get; set; }
        public int? watchCount { get; set; }
        public bool? isWatching { get; set; }
    }

    public class Issuerestriction
    {
        public Issuerestrictions? Issuerestrictions { get; set; }
        public bool? shouldDisplay { get; set; }
    }

    public class Issuerestrictions
    {
    }

    public class Priority
    {
        public string? self { get; set; }
        public string? iconUrl { get; set; }
        public string? name { get; set; }
        public string? id { get; set; }
    }

    public class Customfield_10018
    {
        public bool? hasEpicLinkFieldDependency { get; set; }
        public bool? showField { get; set; }
        public Noneditablereason? nonEditableReason { get; set; }
    }

    public class Noneditablereason
    {
        public string? reason { get; set; }
        public string? message { get; set; }
    }

    public class Status
    {
        public string? self { get; set; }
        public string? description { get; set; }
        public string? iconUrl { get; set; }
        public string? name { get; set; }
        public string? id { get; set; }
        public Statuscategory? statusCategory { get; set; }
    }

    public class Statuscategory
    {
        public string? self { get; set; }
        public int? id { get; set; }
        public string? key { get; set; }
        public string? colorName { get; set; }
        public string? name { get; set; }
    }

    public class Timetracking
    {
    }

    public class Creator
    {
        public string? self { get; set; }
        public string? accountId { get; set; }
        public Avatarurls2? avatarUrls { get; set; }
        public string? displayName { get; set; }
        public bool? active { get; set; }
        public string? timeZone { get; set; }
        public string? accountType { get; set; }
    }

    public class Avatarurls2
    {
        public string? _48x48 { get; set; }
        public string? _24x24 { get; set; }
        public string? _16x16 { get; set; }
        public string? _32x32 { get; set; }
    }

    public class Reporter
    {
        public string? self { get; set; }
        public string? accountId { get; set; }
        public Avatarurls3? avatarUrls { get; set; }
        public string? displayName { get; set; }
        public bool? active { get; set; }
        public string? timeZone { get; set; }
        public string? accountType { get; set; }
    }

    public class Avatarurls3
    {
        public string? _48x48 { get; set; }
        public string? _24x24 { get; set; }
        public string? _16x16 { get; set; }
        public string? _32x32 { get; set; }
    }

    public class Aggregateprogress
    {
        public int? progress { get; set; }
        public int? total { get; set; }
    }

    public class Progress
    {
        public int? progress { get; set; }
        public int? total { get; set; }
    }

    public class Votes
    {
        public string? self { get; set; }
        public int? votes { get; set; }
        public bool? hasVoted { get; set; }
    }

    public class Changelog
    {
        public string? id { get; set; }
        public Item[]? items { get; set; }
    }

    public class Item
    {
        public string? field { get; set; }
        public string? fieldtype { get; set; }
        public string? fieldId { get; set; }
        public string? from { get; set; }
        public string? fromstring { get; set; }
        public string? to { get; set; }
        public string? tostring { get; set; }
    }
}
