
using Newtonsoft.Json;
using Totira.Support.Application.Messages;

namespace Totira.Support.EventServiceBus
{
    public class Envelope<TMessage> where TMessage : IMessage
    {
        public IContext Context { get; }

        public TMessage Message { get; }

        public Envelope(IContext context, TMessage message)
        {
            Context = context;
            Message = message;
        }

        [JsonConstructor]
        public Envelope(Context context, TMessage message) : this((IContext)context, message)
        {
        }
    }
}
