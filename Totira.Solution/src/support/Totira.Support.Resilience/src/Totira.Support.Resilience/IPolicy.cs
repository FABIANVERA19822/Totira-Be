namespace Totira.Support.Resilience
{
    public interface IPolicy
    {
        void Execute(Action action);

        T Execute<T>(Func<T> action);

        Task ExecuteAsync(Func<Task> action);
        Task<T> ExecuteAsync<T>(Func<Task<T>> action);

    }
}
