namespace Totira.Support.Persistance
{
    public interface IAuditable
    {
        Guid CreatedBy { get; }
        DateTimeOffset CreatedOn { get; }
        Guid? UpdatedBy { get; }
        DateTimeOffset? UpdatedOn { get; }
    }
}
