﻿using System.Linq.Expressions;

namespace Commerce.Core.Common;

public static class QueryExtension
{
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

    public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> queryable, int page, int limit)
    {
        if (page > 0 && limit > 0)
        {
            return queryable.Skip((page - 1) * limit).Take(limit);
        }

        return queryable;
    }
}
