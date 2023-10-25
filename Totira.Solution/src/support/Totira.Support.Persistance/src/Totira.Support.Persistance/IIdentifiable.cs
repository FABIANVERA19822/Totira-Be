namespace Totira.Support.Persistance
{
    public interface IIdentifiable<T>
    {
        T Id { get; }
    }
}
