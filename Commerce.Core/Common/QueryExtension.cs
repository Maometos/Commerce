using System.Linq.Expressions;

namespace Commerce.Core.Common;

public static class QueryExtension
{
    public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> queryable, string attribute, object value)
    {
        var type = typeof(TEntity);
        var property = type.GetProperty(attribute);

        if (property != null)
        {
            var parameter = Expression.Parameter(type, "e");
            var member = Expression.MakeMemberAccess(parameter, property);
            var constant = Expression.Constant(value);
            var equality = Expression.Equal(member, constant);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

            queryable = queryable.Where(lambda);
        }

        return queryable;
    }

    public static IQueryable<TEntity> Sort<TEntity>(this IQueryable<TEntity> queryable, string attribute, bool reverse = false)
    {
        var type = typeof(TEntity);
        var property = type.GetProperty(attribute);

        if (property != null)
        {
            var parameter = Expression.Parameter(type);
            return queryable.Provider.CreateQuery<TEntity>(
                Expression.Call(
                    typeof(Queryable),
                    reverse == false ? "OrderBy" : "OrderByDescending",
                    [type, property.PropertyType],
                    queryable.Expression,
                    Expression.Quote(Expression.Lambda(Expression.Property(parameter, property), parameter))));
        }

        return queryable;
    }
}
