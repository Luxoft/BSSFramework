using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

using NHibernate.Linq;

namespace Framework.DomainDriven.NHibernate;

internal static class QueryableExtensions
{
    public static IQueryable<TDomainObject> WithFetchs<TDomainObject>(this IQueryable<TDomainObject> queryable, IFetchContainer<TDomainObject> fetchContainer)
    {
        if (queryable == null) throw new ArgumentNullException(nameof(queryable));

        if (fetchContainer == null || !fetchContainer.Fetchs.Any())
        {
            return queryable;
        }
        else
        {
            var func = FetchHelper<TDomainObject>.Cache[fetchContainer//.Compress()
                                                        .Fetchs.SelectMany(fetch => fetch.ToPropertyPaths()).ToReadOnlyCollection()];

            return func(queryable);
        }
    }


    private static Expression<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> GetWithFetchsExpr<TDomainObject> (this IEnumerable<PropertyPath> paths)
    {
        var sourceParam = Expression.Parameter(typeof (IQueryable<TDomainObject>));

        var body = paths.Aggregate((Expression) sourceParam, (expr, propertyPath) =>
                                                             {
                                                                 var internalParam = (Expression)Expression.Parameter(typeof (IQueryable<TDomainObject>));


                                                                 var queryPath = propertyPath.Aggregate(internalParam, ApplyFetch);


                                                                 return queryPath.Override(internalParam, expr);
                                                             });


        var result = Expression.Lambda<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>>(body, sourceParam);

        return result;
    }

    private static Expression ApplyFetch(this Expression queryExpression, PropertyInfo property)
    {
        if (queryExpression == null) throw new ArgumentNullException(nameof(queryExpression));
        if (property == null) throw new ArgumentNullException(nameof(property));

        var isContinue = queryExpression.Type.GetGenericTypeDefinition() == typeof(INhFetchRequest<,>);
        var isCollection = property.PropertyType.IsCollection();

        var relatedType = isCollection ? property.PropertyType.GetCollectionElementType() : property.PropertyType;

        var methodName = isContinue ? isCollection ? "ThenFetchMany" : "ThenFetch"
                         : isCollection ? "FetchMany"     : "Fetch";

        var baseMethod = typeof(EagerFetchingExtensionMethods).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, true);

        var method = baseMethod.MakeGenericMethod(queryExpression.Type.GetGenericArguments().Concat(new[] { relatedType }).ToArray());

        var sourceParameter = Expression.Parameter(queryExpression.Type.GetGenericArguments()[isContinue ? 1 : 0]);

        var lambda = Expression.Lambda(Expression.Property(sourceParameter, property), sourceParameter);

        return Expression.Call(method, queryExpression, lambda);
    }


    private static class FetchHelper<TDomainObject>
    {
        public static readonly IDictionaryCache<ReadOnlyCollection<PropertyPath>, Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> Cache = new DictionaryCache<ReadOnlyCollection<PropertyPath>, Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>>(paths =>
        {
            var fetchExpr = paths.GetWithFetchsExpr<TDomainObject>();

            return fetchExpr.Compile();

        }, SequenceComparer<PropertyPath>.Value).WithLock();
    }
}
