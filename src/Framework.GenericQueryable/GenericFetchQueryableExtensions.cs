using System.Linq.Expressions;

namespace Framework.GenericQueryable;

public static class GenericFetchQueryableExtensions
{
    public static IQueryable<TSource> WithFetch<TSource>(this IQueryable<TSource> source, string fetchPath)
    {
        Expression<Func<IQueryable<TSource>>> callExpression = () => source.WithFetch(fetchPath);

        return source.Provider.Execute<IQueryable<TSource>>(new GenericQueryableExecuteExpression(callExpression));
    }
}

//public interface IFetchQueryable<out TEntity, out TProperty> : IQueryable<TEntity>
//{

//}

//public class FetchQueryable
