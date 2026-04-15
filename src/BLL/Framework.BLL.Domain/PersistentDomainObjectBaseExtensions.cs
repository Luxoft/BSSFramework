using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using Framework.Application.Domain;
using Framework.Core;

namespace Framework.BLL.Domain;

public static class PersistentDomainObjectBaseExtensions
{
    public static TIdent? TryGetId<TIdent>(this IIdentityObject<TIdent> source) => source.Maybe(v => v.Id);

    /// <summary>
    /// Sets value one time.
    /// If you try set value again then you get BusinessLogicException
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TField"></typeparam>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="source">Domain object</param>
    /// <param name="fieldExpr">Field expression</param>
    /// <param name="newValue">New value for field</param>
    public static void SetValueSafe<TSource, TField, TIdent>(this TSource source, Expression<Func<TSource, TField>> fieldExpr, TField newValue)
            where TSource : class, IIdentityObject<TIdent>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (fieldExpr == null) throw new ArgumentNullException(nameof(fieldExpr));

        var field = (fieldExpr.Body as MemberExpression)
                    .Maybe(expr => expr.Member as FieldInfo)
                    .FromMaybe(() => new ArgumentException("Invalid field expression", nameof(fieldExpr)));


        if (!Equals(field.GetValue(source), newValue))
        {
            if (!source.Id.IsDefault())
            {
                throw new InvalidOperationException($"Field \"{field.Name}\" of object \"{source.GetType().Name}\" can't be changed because already initialized");
            }

            field.SetValue(source, newValue);
        }
    }

    /// <summary>
    /// Sets value one time.
    /// If you try set value again then you get BusinessLogicException
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="source">Domain object</param>
    /// <param name="propertyExpr">Property expression</param>
    /// <param name="newValue">New value for property</param>
    /// <param name="customSetAction">Action for setter</param>
    public static void SetValueSafe<TSource, TProperty, TIdent>(this TSource source, Expression<Func<TSource, TProperty>> propertyExpr, TProperty newValue, Action customSetAction)
            where TSource : class, IIdentityObject<TIdent>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (propertyExpr == null) throw new ArgumentNullException(nameof(propertyExpr));
        if (customSetAction == null) throw new ArgumentNullException(nameof(customSetAction));

        var property = (propertyExpr.Body as MemberExpression)
                       .Maybe(expr => expr.Member as PropertyInfo)
                       .FromMaybe(() => new ArgumentException("Invalid property expression", nameof(propertyExpr)));


        if (!Equals(property.GetValue(source, null), newValue))
        {
            if (!source.Id.IsDefault())
            {
                throw new InvalidOperationException($"Property \"{property.Name}\" of object \"{source.GetType().Name}\" can't be changed because already initialized");
            }

            customSetAction();
        }
    }


    public static TProperty? GetOneToOne<TSource, TProperty, TIdent>(this TSource source, Expression<Func<TSource, IEnumerable<TProperty>>> propertyExpr)
            where TSource : class, IIdentityObject<TIdent>
            where TProperty : class, IIdentityObject<TIdent>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (propertyExpr == null) throw new ArgumentNullException(nameof(propertyExpr));

        return source.GetCollectionValue(propertyExpr)
                     .Match(() => null,
                            v  => v,
                            _ => { throw new Exception($"{typeof(TSource).Name} one-to-one error. To many items in collection {typeof(TProperty).Name}"); });
    }

    public static void SetOneToOne<TSource, TProperty, TIdent>(this TSource source, Expression<Func<TSource, IEnumerable<TProperty>>> propertyExpr, TProperty? newValue)
            where TSource : class, IIdentityObject<TIdent>
            where TProperty : class, IIdentityObject<TIdent>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (propertyExpr == null) throw new ArgumentNullException(nameof(propertyExpr));

        if (!EqualityComparer<TProperty>.Default.Equals(source.GetOneToOne<TSource, TProperty, TIdent>(propertyExpr), newValue))
        {
            var collection = source.GetCollectionValue(propertyExpr);

            if (collection.Count > 1)
            {
                throw new Exception($"{typeof(TSource).Name} one-to-one error. To many items in collection {typeof(TProperty).Name}");
            }

            collection.Clear();

            if (newValue != null)
            {
                collection.Add(newValue);
            }
        }
    }

    private static ICollection<TProperty> GetCollectionValue<TSource, TProperty>(this TSource source, Expression<Func<TSource, IEnumerable<TProperty>>> propertyExpr)
    {
        if (propertyExpr == null) throw new ArgumentNullException(nameof(propertyExpr));

        var collectionRequest = from expr in (propertyExpr.Body as MemberExpression).ToMaybe()

                                from property in (expr.Member as PropertyInfo).ToMaybe()

                                from collection in (property.GetValue(source, null) as ICollection<TProperty>).ToMaybe()

                                select collection;

        return collectionRequest.GetValue(() => new ArgumentException("Invalid property expression", nameof(propertyExpr)));
    }
}
