using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core;

public static class PropertyPathExtensions
{
    public static IEnumerable<Node<PropertyInfo>> ToNodes(this IEnumerable<PropertyPath> paths)
    {
        if (paths == null) throw new ArgumentNullException(nameof(paths));

        return from path in paths

               where !path.IsEmpty

               group path.Tail by path.Head into g

               select new Node<PropertyInfo>(g.Key, g.ToNodes());
    }

    public static bool HasReferenceResult(this PropertyPath path)
    {
        if (path == null) { throw new ArgumentNullException(nameof(path)); }

        return path.Any(prop => prop.PropertyType.IsClass || prop.PropertyType.IsNullable());
    }

    /// <summary>
    /// Получение всех возможных путей из ноды
    /// </summary>
    /// <param name="node">Узел</param>
    /// <returns></returns>
    public static IEnumerable<PropertyPath> ToPaths(this Node<PropertyInfo> node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        if (node.Children.Any())
        {
            return node.Children.SelectMany(subNode => subNode.ToPaths()).Select(subPath => node.Value + subPath);
        }
        else
        {
            return new[] { new PropertyPath(new[] { node.Value }) };
        }
    }

    /// <summary>
    /// Приведение пути к лямбде
    /// </summary>
    /// <param name="propertyPath">Путь</param>
    /// <param name="sourceType">Стартовый тип от которого строится лямбда</param>
    /// <returns></returns>
    public static LambdaExpression ToLambdaExpression(this PropertyPath propertyPath, Type sourceType = null)
    {
        if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));

        var rootParam = Expression.Parameter(sourceType ?? propertyPath.Head.ReflectedType);

        return propertyPath.Aggregate((Expression)rootParam, (state, prop) => Expression.Property(state, prop), res => Expression.Lambda(res, rootParam));
    }

    /// <summary>
    /// Приведение колллекции свойств к пути
    /// </summary>
    /// <param name="properties">Коллекция свойств</param>
    /// <returns></returns>
    public static PropertyPath ToPropertyPath(this IEnumerable<PropertyInfo> properties)
    {
        if (properties == null) throw new ArgumentNullException(nameof(properties));

        return new PropertyPath(properties);
    }
}
