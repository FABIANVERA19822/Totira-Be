
namespace Totira.Services.Thirdparty.Worker.Outbox.Bll.DTO
{
    public class TenantGroupApplicationProfileDto
    {
        public TenantGroupApplicationProfileDto() { }
        public TenantGroupApplicationProfileDto(
            Guid tenantId,
            string firstName,
            string email,
            int invinteeType,
            int status,
            bool isVerifiedEmailConfirmation,
            DateTimeOffset createdOn)
        {
            TenantId = tenantId;
            FirstName = firstName;
            Email = email;
            InvinteeType = invinteeType;
            Status = status;
            IsVerifiedEmailConfirmation = isVerifiedEmailConfirmation;
            CreatedOn = createdOn;
        }
        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int InvinteeType { get; set; }
        public int Status { get; set; } = 1;
        public bool IsVerifiedEmailConfirmation { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
