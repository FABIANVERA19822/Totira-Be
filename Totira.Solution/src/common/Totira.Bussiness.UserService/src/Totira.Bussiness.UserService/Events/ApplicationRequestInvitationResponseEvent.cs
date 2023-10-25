using Totira.Support.Application.Events;

namespace Totira.Bussiness.UserService.Events
{
    public class ApplicationRequestInvitationResponseEvent : IEvent
    {
        public Guid Id { get; set; }

        public ApplicationRequestInvitationResponseEvent(Guid id) => Id = id;
    }
}
