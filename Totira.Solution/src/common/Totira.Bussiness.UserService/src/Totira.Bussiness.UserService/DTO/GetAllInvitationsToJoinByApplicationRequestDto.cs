namespace Totira.Bussiness.UserService.DTO
{
    public class GetAllInvitationsToJoinByApplicationRequestDto
    {
        public Guid ApplicationRequestId { get; set; }
        public List<GetAllInvitationstoJoin>? Invitations { get; set; }
    }

    public class GetAllInvitationstoJoin
    {
        public Guid Id { get; set; }
        public string CoapplicantEmail { get; set; }
        public bool IsActived { get; set; }
        public DateTimeOffset? UpdateOn { get; set; }
    }
}
