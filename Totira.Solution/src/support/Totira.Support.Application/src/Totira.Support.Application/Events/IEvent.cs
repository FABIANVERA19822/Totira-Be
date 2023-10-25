using Totira.Support.Application.Messages;

namespace Totira.Support.Application.Events
{
    public interface IEvent : IMessage
    {
    }

    public abstract class BaseValidatedEvent : IEvent
    {
        public bool IsValid => (Errors?.Count() ?? 0) == 0;
        public IEnumerable<string>? Errors { get; init; }
    }
}
