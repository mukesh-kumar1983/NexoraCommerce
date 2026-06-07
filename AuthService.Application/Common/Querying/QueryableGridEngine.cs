using System.Linq.Expressions;

namespace NexoraEnterprise.AuthService.Application.Common.Querying;

public static class QueryableGridEngine
{
    /// <summary>
    /// EF Core TRANSLATABLE search (IMPORTANT FIX)
    /// </summary>
    public static IQueryable<T> ApplySearch<T>(
        this IQueryable<T> query,
        string? search,
        params Expression<Func<T, string>>[] searchFields)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        search = search.Trim().ToLower();

        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? body = null;

        foreach (var field in searchFields)
        {
            var invoked = Expression.Invoke(field, parameter);

            var toLower = Expression.Call(
                invoked,
                typeof(string).GetMethod("ToLower", Type.EmptyTypes)!);

            var contains = Expression.Call(
                toLower,
                typeof(string).GetMethod("Contains", new[] { typeof(string) })!,
                Expression.Constant(search));

            body = body == null ? contains : Expression.OrElse(body, contains);
        }

        var lambda = Expression.Lambda<Func<T, bool>>(body!, parameter);

        return query.Where(lambda);
    }

    /// <summary>
    /// Sorting (already correct)
    /// </summary>
    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        string? sortField,
        string? sortDir,
        Dictionary<string, Expression<Func<T, object>>> sortMap)
    {
        if (string.IsNullOrWhiteSpace(sortField))
            return query;

        if (!sortMap.TryGetValue(sortField.ToLower(), out var selector))
            return query;

        return sortDir?.ToLower() == "desc"
            ? query.OrderByDescending(selector)
            : query.OrderBy(selector);
    }
}