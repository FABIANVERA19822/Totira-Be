using Totira.Bussiness.UserService.Enums;

namespace Totira.Bussiness.UserService.DTO
{
    public class TenantGroupApplicationProfileDto
    {
        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int InvinteeType { get; set; }
        public int Status { get; set; } = (int)InvitationStatus.Pending;
        public bool IsVerifiedEmailConfirmation { get; set; } 
    }
}
