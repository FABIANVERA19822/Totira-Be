using System.Linq.Expressions;
using System.Reflection;
using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Extensions.DomainExtensions;

public static class GetExtensions
{
    public static async Task<T?> LastOrDefault<T>(this IRepository<T, Guid> repository, Expression<Func<T, bool>> predicate)
        where T : Document
    {
        var coincidences = await repository.Get(predicate);

        if (coincidences.Any() && typeof(T).GetInterfaces().Contains(typeof(IAuditable)))
        {
            PropertyInfo? createdOnProperty = typeof(T).GetProperty("CreatedOn");
            if (createdOnProperty is not null)
                return coincidences.MaxBy(x => (DateTimeOffset)createdOnProperty.GetValue(x)!);
        }

        return default;
    }
}