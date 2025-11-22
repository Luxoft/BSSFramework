using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using GenericQueryable;
using GenericQueryable.Fetching;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Linq;

namespace Framework.DomainDriven.NHibernate;

public class NHibGenericQueryableExecutor(IServiceProvider serviceProvider) : GenericQueryableExecutor
{
    protected override IReadOnlyList<Type> ExtensionsTypes { get; } = [typeof(LinqExtensionMethods)];

    public override Task<TResult> ExecuteAsync<TResult>(Expression<Func<Task<TResult>>> expression)
    {
        return base.Execute<Task<TResult>>(expression);
    }

    public override IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchRule<TSource> fetchRule)
    {
        return fetchRule switch
        {
            UntypedFetchRule<TSource> untypedFetchRule => serviceProvider.GetRequiredService<INHibFetchService<TSource>>().ApplyFetch(source, untypedFetchRule.Path),

            PropertyFetchRule<TSource> propertyFetchRule => this.ApplyFetch(source, propertyFetchRule),

            _ => throw new ArgumentOutOfRangeException(nameof(fetchRule))
        };
    }

    protected IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, PropertyFetchRule<TSource> fetchRule)
        where TSource : class
    {
        return fetchRule.Paths.Aggregate(source, this.ApplyFetch);
    }

    private IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, FetchPath fetchPath)
        where TSource : class
    {
        return fetchPath
               .Properties
               .Select((prop, index) => new { Prop = prop, IsFirst = index == 0 })
               .Aggregate(source, (q, pair) => this.ApplyFetch(q, pair.Prop, pair.IsFirst));
    }

    private IQueryable<TSource> ApplyFetch<TSource>(IQueryable<TSource> source, LambdaExpression prop, bool isFirst)
        where TSource : class
    {
        return this.GetFetchMethod<TSource>(prop, isFirst).Invoke<IQueryable<TSource>>(this, source, prop);
    }


    private MethodInfo GetFetchMethod<TSource>(LambdaExpression prop, bool isFirst)
        where TSource : class
    {
        var prevPropertyType = prop.Parameters.Single().Type;

        var nextPropertyType = prop.Body.Type;

        var nextElementType = nextPropertyType.IsGenericType ? nextPropertyType.GetInterfaceImplementationArgument(typeof(IEnumerable<>)) : null;

        if (isFirst)
        {
            if (nextElementType != null)
            {
                return new Func<IQueryable<TSource>, Expression<Func<TSource, IEnumerable<Ignore>>>, INhFetchRequest<TSource, Ignore>>(this.ApplyFetch)
                    .CreateGenericMethod(typeof(TSource), nextElementType);
            }
            else
            {
                return new Func<IQueryable<TSource>, Expression<Func<TSource, Ignore>>, INhFetchRequest<TSource, Ignore>>(this.ApplyFetch)
                    .CreateGenericMethod(typeof(TSource), nextPropertyType);
            }
        }
        else
        {
            if (nextElementType != null)
            {
                return new Func<INhFetchRequest<TSource, Ignore>, Expression<Func<Ignore, IEnumerable<Ignore>>>, INhFetchRequest<TSource, Ignore>>(
                        this.ApplyFetchThen)
                    .CreateGenericMethod(typeof(TSource), prevPropertyType, nextElementType);
            }
            else
            {
                return new Func<INhFetchRequest<TSource, Ignore>, Expression<Func<Ignore, Ignore>>, INhFetchRequest<TSource, Ignore>>(
                        this.ApplyFetchThen)
                    .CreateGenericMethod(typeof(TSource), prevPropertyType, nextPropertyType);
            }
        }
    }

    private INhFetchRequest<TSource, TProperty> ApplyFetch<TSource, TProperty>(IQueryable<TSource> source, Expression<Func<TSource, TProperty>> prop)
        where TSource : class
    {
        return source.Fetch(prop);
    }
    private INhFetchRequest<TSource, TProperty> ApplyFetch<TSource, TProperty>(IQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TProperty>>> prop)
        where TSource : class
    {
        return source.FetchMany(prop);
    }

    private INhFetchRequest<TSource, TNextProperty> ApplyFetchThen<TSource, TPrevProperty, TNextProperty>(
        INhFetchRequest<TSource, TPrevProperty> source,
        Expression<Func<TPrevProperty, TNextProperty>> prop)
        where TSource : class
    {
        return source.ThenFetch(prop);
    }

    private INhFetchRequest<TSource, TNextProperty> ApplyFetchThen<TSource, TPrevProperty, TNextProperty>(
        INhFetchRequest<TSource, TPrevProperty> source,
        Expression<Func<TPrevProperty, IEnumerable<TNextProperty>>> prop)
        where TSource : class
    {
        return source.ThenFetchMany(prop);
    }
}
