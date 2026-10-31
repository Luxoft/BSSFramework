using Anch.Core;

using Framework.Core;
using Framework.Core.Helpers;

namespace Framework.Projection;

public static class TypeExtensions
{
    /// <summary>
    /// Getting the list of projection filters
    /// </summary>
    /// <param name="projectionType">Projection type</param>
    /// <param name="target">Filter applicability</param>
    /// <returns></returns>
    public static IEnumerable<Type> GetProjectionFilters(this Type projectionType, ProjectionFilterTargets target = ProjectionFilterTargets.All)
    {
        if (projectionType is null) throw new ArgumentNullException(nameof(projectionType));

        return projectionType.GetCustomAttributes<ProjectionFilterAttribute>()
                             .Where(attr => EnumHelper.GetValues<ProjectionFilterTargets>().Any(v => target.HasFlag(v) && attr.Target.HasFlag(v)))
                             .Select(attr => attr.FilterType);
    }

    /// <summary>
    /// Getting the source type of the projection by its contract
    /// </summary>
    /// <param name="contractType">Projection contract type</param>
    /// <param name="raiseIfNull">Existence check</param>
    /// <returns></returns>
    public static Type? GetProjectionContractSourceType(this Type contractType, bool raiseIfNull = true)
    {
        if (contractType is null) throw new ArgumentNullException(nameof(contractType));

        var attr = contractType.GetCustomAttribute<ProjectionContractAttribute>();

        if (attr is null)
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
    /// Getting the source type of the projection
    /// </summary>
    /// <param name="type">Projection type</param>
    /// <param name="raiseIfNull">Existence check</param>
    /// <returns></returns>
    public static Type? GetProjectionSourceType(this Type type, bool raiseIfNull = true)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var attr = type.GetCustomAttribute<ProjectionAttribute>();

        if (attr is null)
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
    /// Getting the source type of the projection or itself
    /// </summary>
    /// <param name="type">Projection type</param>
    /// <returns></returns>
    public static Type GetProjectionSourceTypeOrSelf(this Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetProjectionSourceType(false) ?? type;
    }

    /// <summary>
    /// Getting the contract from the projection
    /// </summary>
    /// <param name="type">Projection type</param>
    /// <param name="raiseIfNull">Existence check</param>
    /// <returns></returns>
    public static Type? GetProjectionContractType(this Type type, bool raiseIfNull = true)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var attr = type.GetCustomAttribute<ProjectionAttribute>();

        if (attr is null)
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
    /// Check that the type is a projection
    /// </summary>
    /// <param name="type">Projection type</param>
    /// <returns></returns>
    public static bool IsProjection(this Type type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetProjectionSourceType(false) is not null;
    }

    /// <summary>
    /// Check that the type is a projection with a condition on the base type
    /// </summary>
    /// <param name="type">Projection type</param>
    /// <param name="condition">Condition on the base type</param>
    /// <returns></returns>
    public static bool IsProjection(this Type type, Func<Type, bool> condition)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetProjectionSourceType(false).Maybe(condition);
    }
}
