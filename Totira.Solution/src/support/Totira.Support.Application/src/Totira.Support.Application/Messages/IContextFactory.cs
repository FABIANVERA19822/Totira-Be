namespace Totira.Support.Application.Messages
{
    public interface IContextFactory
    {
        IContext Create();
        IContext Create(string href, Guid userId);
    }
}
