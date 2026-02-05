using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

namespace Framework.Core;

public static class PropertyPathExtensions
{
    public static bool HasReferenceResult(this PropertyPath path)
    {
        if (path == null) { throw new ArgumentNullException(nameof(path)); }

        return path.Any(prop => prop.PropertyType.IsClass || prop.PropertyType.IsNullable());
    }

    public static PropertyPath ToPropertyPath(this IEnumerable<PropertyInfo> properties)
    {
        if (properties == null) throw new ArgumentNullException(nameof(properties));

        return new PropertyPath(properties);
    }

    /// <summary>
    /// Приведение пути к лямбде
    /// </summary>
    /// <param name="propertyPath">Путь</param>
    /// <param name="sourceType">Стартовый тип от которого строится лямбда</param>
    /// <returns></returns>
    public static LambdaExpression ToLambdaExpression(this PropertyPath propertyPath, Type? sourceType = null)
    {
        if (propertyPath == null) throw new ArgumentNullException(nameof(propertyPath));

        var rootParam = Expression.Parameter(sourceType ?? propertyPath.Head.ReflectedType!);

        return propertyPath.Aggregate((Expression)rootParam, Expression.Property, res => Expression.Lambda(res, rootParam));
    }

    public static IEnumerable<PropertyPath> Compress(this IEnumerable<PropertyPath> paths)
    {
        var resultPaths = new List<PropertyPath>();

        paths.Foreach(path =>
        {
            var overridePath = resultPaths.FirstOrDefault(path.StartsWith);

            if (overridePath is null)
            {
                if (resultPaths.All(rPath => !rPath.StartsWith(path)))
                {
                    resultPaths.Add(path);
                }
            }
            else
            {
                resultPaths.Remove(overridePath);
                resultPaths.Add(path);
            }
        });

        return resultPaths;
    }
}
