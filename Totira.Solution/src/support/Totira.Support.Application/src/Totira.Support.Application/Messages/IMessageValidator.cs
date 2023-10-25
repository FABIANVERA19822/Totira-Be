namespace Totira.Support.Application.Messages
{
    public interface IMessageValidator<TMessage> where TMessage : IMessage
    {
        ValidationResult Validate(TMessage message);
    }
}
