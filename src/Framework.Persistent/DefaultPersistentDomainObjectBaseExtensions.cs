using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Framework.Persistent;

public static class DefaultPersistentDomainObjectBaseExtensions
{
    public static TProperty GetOneToOne<TSource, TProperty>(this TSource source, Expression<Func<TSource, IEnumerable<TProperty>>> fieldExpr)
            where TSource : class, IIdentityObject<Guid>
            where TProperty : class, IIdentityObject<Guid>
    {
        return source.GetOneToOne<TSource, TProperty, Guid>(fieldExpr);
    }

    public static void SetOneToOne<TSource, TProperty>(this TSource source, Expression<Func<TSource, IEnumerable<TProperty>>> fieldExpr, TProperty newValue)
            where TSource : class, IIdentityObject<Guid>
            where TProperty : class, IIdentityObject<Guid>
    {
        source.SetOneToOne<TSource, TProperty, Guid>(fieldExpr, newValue);
    }

    /// <summary>
    /// Sets value one time.
    /// If you try set value again then you get BusinessLogicException
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TField"></typeparam>
    /// <param name="source">Domain object</param>
    /// <param name="fieldExpr">Field expression</param>
    /// <param name="newValue">New value for field</param>
    public static void SetValueSafe<TSource, TField>(this TSource source, Expression<Func<TSource, TField>> fieldExpr, TField newValue)
            where TSource : class, IIdentityObject<Guid>
    {
        source.SetValueSafe<TSource, TField, Guid>(fieldExpr, newValue);
    }

    /// <summary>
    /// Sets value one time.
    /// If you try set value again then you get BusinessLogicException
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TField"></typeparam>
    /// <param name="source">Domain object</param>
    /// <param name="fieldExpr">Field expression</param>
    /// <param name="newValue">New value for field</param>
    /// <param name="customSetAction">Action for setter</param>
    public static void SetValueSafe<TSource, TField>(this TSource source, Expression<Func<TSource, TField>> fieldExpr, TField newValue, Action customSetAction)
            where TSource : class, IIdentityObject<Guid>
    {
        source.SetValueSafe<TSource, TField, Guid>(fieldExpr, newValue, customSetAction);
    }
}
