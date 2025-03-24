using Framework.Core;

using NHibernate.Linq;

using System.Linq.Expressions;
using System.Reflection;

namespace Framework.DomainDriven.NHibernate;

public class NHibFetchService<TSource> : INHibFetchService<TSource>
{
    private readonly IDictionaryCache<string, Func<IQueryable<TSource>, IQueryable<TSource>>> cache =
        new DictionaryCache<string, Func<IQueryable<TSource>, IQueryable<TSource>>>(
            path =>
            {
                var propertyPath = PropertyPath.Create(typeof(TSource), path.Split('.'));

                var fetchExp = GetWithFetchExpr([propertyPath]);

                return fetchExp.Compile();

            }).WithLock();

    public IQueryable<TSource> ApplyFetch(IQueryable<TSource> source, string path)
    {
        return this.cache[path].Invoke(source);
    }

    public static Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> GetWithFetchExpr(IEnumerable<PropertyPath> paths)
    {
        var sourceParam = Expression.Parameter(typeof(IQueryable<TSource>));

        var body = paths.Aggregate((Expression)sourceParam, (expr, propertyPath) =>
        {
            var internalParam = (Expression)Expression.Parameter(typeof(IQueryable<TSource>));

            var queryPath = propertyPath.Aggregate(internalParam, ApplyFetch);


            return queryPath.Override(internalParam, expr);
        });


        var result = Expression.Lambda<Func<IQueryable<TSource>, IQueryable<TSource>>>(body, sourceParam);

        return result;
    }

    private static Expression ApplyFetch(Expression queryExpression, PropertyInfo property)
    {
        if (queryExpression == null) throw new ArgumentNullException(nameof(queryExpression));
        if (property == null) throw new ArgumentNullException(nameof(property));

        var isContinue = queryExpression.Type.GetGenericTypeDefinition() == typeof(INhFetchRequest<,>);
        var isCollection = property.PropertyType.IsCollection();

        var relatedType = isCollection ? property.PropertyType.GetCollectionElementType() : property.PropertyType;

        var methodName = isContinue ? isCollection ? "ThenFetchMany" : "ThenFetch"
                         : isCollection ? "FetchMany" : "Fetch";

        var baseMethod = typeof(EagerFetchingExtensionMethods).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public, true);

        var method = baseMethod.MakeGenericMethod(queryExpression.Type.GetGenericArguments().Concat(new[] { relatedType }).ToArray());

        var sourceParameter = Expression.Parameter(queryExpression.Type.GetGenericArguments()[isContinue ? 1 : 0]);

        var lambda = Expression.Lambda(Expression.Property(sourceParameter, property), sourceParameter);

        return Expression.Call(method, queryExpression, lambda);
    }
}
