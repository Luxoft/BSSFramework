using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public static class RelativeDomainPathInfoExtensions
{
    public static IRelativeDomainPathInfo<TFrom, TNewTo> Select<TFrom, TOldTo, TNewTo>(
        this IRelativeDomainPathInfo<TFrom, TOldTo> relativeDomainPathInfo,
        Expression<Func<TOldTo, TNewTo>> selector)
    {
        return new RelativeDomainPathInfo<TFrom, TNewTo>(relativeDomainPathInfo.Path.Select(selector));
    }
}
