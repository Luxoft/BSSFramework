using System.Collections.ObjectModel;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryLanguage;

using LambdaExpression = Framework.QueryLanguage.LambdaExpression;
using ParameterExpression = Framework.QueryLanguage.ParameterExpression;

namespace Framework.OData;

public static class ExpressionExtensions
{
    public static IEnumerable<Tuple<string, string>> GetPropertyPath(this LambdaExpression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        var startParam = expression.Parameters.Single();

        var startProp = ((PropertyExpression)expression.Body);

        var path = startProp.GetAllElements(prop => (prop.Source as ParameterExpression).Maybe(s => s.Name == startParam.Name)
                                                            ? null
                                                            : ((PropertyExpression)prop.Source));

        return path.Reverse().Select(expr => Tuple.Create(expr.PropertyName, (expr as SelectExpression).Maybe(selectExpr => selectExpr.Alias)));
    }
}

internal static class PropertyInfoExtensions
{
    private static readonly ReadOnlyCollection<PropertyInfo> SystemVirtualProperties =

            typeof(DateTime).GetProperties(BindingFlags.Instance | BindingFlags.Public).ToReadOnlyCollection();


    private static readonly IDictionaryCache<PropertyInfo, bool> HasSystemOrPrivateFieldCache =

            new DictionaryCache<PropertyInfo, bool>(property => property.IsSystemVirtualProperty() || property.HasPrivateField(true) || property.HasAttribute<ExpandPathAttribute>()).WithLock();

    private static readonly IDictionaryCache<Type, IEnumerable<Type>> BaseTypesCache = new DictionaryCache<Type, IEnumerable<Type>>(type => type.GetAllElements(q => q.BaseType).TakeWhile(q => typeof(object) != q).ToHashSet(x => x)).WithLock();



    private static bool IsSystemVirtualProperty(this PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        if (SystemVirtualProperties.Contains(property))
        {
            return true;
        }

        if (property.DeclaringType.IsNullable())
        {
            return true;
        }

        return false;
    }

    public static bool HasSystemOrPrivateField(this PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return HasSystemOrPrivateFieldCache[property];
    }

    public static bool InSameHierarchy(this Type arg1, Type arg2)
    {
        return BaseTypesCache[arg1].Contains(arg2);
    }
}
