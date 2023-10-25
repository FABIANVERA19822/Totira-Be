namespace Totira.Bussiness.UserService.DTO.ThirdpartyService
{
    public class GetJiraEmployementTicketDto
    {
        public Guid TenantId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string FinalDecision { get; set; }
        public List<string> Comments { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
