using Framework.Core;
using Framework.Projection.Contract;

using CommonFramework;

namespace Framework.Projection;

public static class TypeExtensions
{
    /// <summary>
    /// Получние списка фильтров проекции
    /// </summary>
    /// <param name="projectionType">Тип проекции</param>
    /// <param name="target">Применимость фильтра</param>
    /// <returns></returns>
    public static IEnumerable<Type> GetProjectionFilters(this Type projectionType, ProjectionFilterTargets target = ProjectionFilterTargets.All)
    {
        if (projectionType == null) throw new ArgumentNullException(nameof(projectionType));

        return projectionType.GetCustomAttributes<ProjectionFilterAttribute>()
                             .Where(attr => EnumHelper.GetValues<ProjectionFilterTargets>().Any(v => target.HasFlag(v) && attr.Target.HasFlag(v)))
                             .Select(attr => attr.FilterType);
    }

    /// <summary>
    /// Получение исходного типа проекции по её контракту
    /// </summary>
    /// <param name="contractType">Тип контракта проекции</param>
    /// <param name="raiseIfNull">Проверка на существоание</param>
    /// <returns></returns>
    public static Type GetProjectionContractSourceType(this Type contractType, bool raiseIfNull = true)
    {
        if (contractType == null) throw new ArgumentNullException(nameof(contractType));

        var attr = contractType.GetCustomAttribute<ProjectionContractAttribute>();

        if (attr == null)
        {
            if (raiseIfNull)
            {
                throw new Exception($"Type {contractType.Name} is not ProjectionContract");
            }
            else
            {
                return null;
            }
        }
        else
        {
            var sourceType = attr.SourceType;

            if (!contractType.IsAssignableFrom(sourceType))
            {
                throw new Exception($"Type {sourceType.Name} not implemented contract {contractType.Name}");
            }
            else
            {
                return sourceType;
            }
        }
    }

    /// <summary>
    /// Получение исходного типа проекции
    /// </summary>
    /// <param name="type">Тип проекции</param>
    /// <param name="raiseIfNull">Проверка на существоание</param>
    /// <returns></returns>
    public static Type GetProjectionSourceType(this Type type, bool raiseIfNull = true)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var attr = type.GetCustomAttribute<ProjectionAttribute>();

        if (attr == null)
        {
            if (raiseIfNull)
            {
                throw new Exception($"Type {type.Name} is not Projection");
            }
            else
            {
                return null;
            }
        }
        else
        {
            return attr.SourceType;
        }
    }

    /// <summary>
    /// Получение исходного типа проекции или самого себя
    /// </summary>
    /// <param name="type">Тип проекции</param>
    /// <returns></returns>
    public static Type GetProjectionSourceTypeOrSelf(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetProjectionSourceType(false) ?? type;
    }

    /// <summary>
    /// Получение контрактка из проекции
    /// </summary>
    /// <param name="type">Тип проекции</param>
    /// <param name="raiseIfNull">Проверка на существоание</param>
    /// <returns></returns>
    public static Type GetProjectionContractType(this Type type, bool raiseIfNull = true)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var attr = type.GetCustomAttribute<ProjectionAttribute>();

        if (attr == null)
        {
            if (raiseIfNull)
            {
                throw new Exception($"Type {type.Name} is not Projection");
            }
            else
            {
                return null;
            }
        }
        else
        {
            return attr.ContractType;
        }
    }

    /// <summary>
    /// Проверка типа на то, что он является проекцией
    /// </summary>
    /// <param name="type">Тип проекции</param>
    /// <returns></returns>
    public static bool IsProjection(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetProjectionSourceType(false) != null;
    }

    /// <summary>
    /// Проверка типа на то, что он является проекцией с условием на базовый тип
    /// </summary>
    /// <param name="type">Тип проекции</param>
    /// <param name="condition">Условие на базовый тип</param>
    /// <returns></returns>
    public static bool IsProjection(this Type type, Func<Type, bool> condition)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetProjectionSourceType(false).Maybe(condition);
    }
}
