namespace Totira.Bussiness.UserService.DTO
{
    public class GetApplicationRequestbyInvitationDto
    {
        public Guid Id { get; set; }
        public string CoapplicantEmail { get; set; }
        public Guid ApplicationRequestId { get; set; }
        public bool IsActived { get; set; } = false;
    }
}
