using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using GenericQueryable.Fetching;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Linq;

namespace Framework.DomainDriven.NHibernate;

public class NHibFetchService([FromKeyedServices(RootFetchRuleExpander.Key)]IFetchRuleExpander fetchRuleExpander) : FetchService(fetchRuleExpander)
{
    protected override IEnumerable<MethodInfo> GetFetchMethods<TSource>(LambdaExpressionPath fetchPath) =>
        fetchPath.Properties.Select((prop, index) => GetFetchMethod<TSource>(prop, index == 0));

    private static MethodInfo GetFetchMethod<TSource>(LambdaExpression prop, bool isFirst)
        where TSource : class
    {
        var prevPropertyType = prop.Parameters.Single().Type;

        var nextPropertyType = prop.Body.Type;

        var nextElementType = nextPropertyType.IsGenericType ? nextPropertyType.GetInterfaceImplementationArgument(typeof(IEnumerable<>)) : null;

        if (isFirst)
        {
            if (nextElementType != null)
            {
                return new Func<IQueryable<TSource>, Expression<Func<TSource, IEnumerable<Ignore>>>, INhFetchRequest<TSource, Ignore>>(EagerFetchingExtensionMethods.FetchMany)
                    .CreateGenericMethod(typeof(TSource), nextElementType);
            }
            else
            {
                return new Func<IQueryable<TSource>, Expression<Func<TSource, Ignore>>, INhFetchRequest<TSource, Ignore>>(EagerFetchingExtensionMethods.Fetch)
                    .CreateGenericMethod(typeof(TSource), nextPropertyType);
            }
        }
        else
        {
            if (nextElementType != null)
            {
                return new Func<INhFetchRequest<TSource, Ignore>, Expression<Func<Ignore, IEnumerable<Ignore>>>, INhFetchRequest<TSource, Ignore>>(
                        EagerFetchingExtensionMethods.ThenFetchMany)
                    .CreateGenericMethod(typeof(TSource), prevPropertyType, nextElementType);
            }
            else
            {
                return new Func<INhFetchRequest<TSource, Ignore>, Expression<Func<Ignore, Ignore>>, INhFetchRequest<TSource, Ignore>>(
                        EagerFetchingExtensionMethods.ThenFetch)
                    .CreateGenericMethod(typeof(TSource), prevPropertyType, nextPropertyType);
            }
        }
    }
}
