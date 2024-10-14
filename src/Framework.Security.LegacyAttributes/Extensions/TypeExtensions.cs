using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Security;

public static class TypeExtensions
{
    internal static SecurityRule GetSecurityRule(this Type securityRuleType, string name)
    {
        switch (securityRuleType.GetProperty(name)!.GetValue(null))
        {
            case SecurityRule securityRule:
                return securityRule;

            case SecurityRole securityRole:
                return securityRole;

            case SecurityOperation securityOperation:
                return securityOperation;

            default:
                throw new ArgumentOutOfRangeException(nameof(name));

        }
    }

    public static Type GetDependencySecuritySourceType(this Type type, bool recurse)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var attr = type.GetCustomAttribute<DependencySecurityAttribute>();

        if (attr != null)
        {
            if (recurse)
            {
                return attr.SourceType.GetDependencySecuritySourceType(true) ?? attr.SourceType;
            }

            return attr.SourceType;
        }

        return null;
    }

    public static bool HasSecurityNodeInterfaces(this Type sourceType)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

        return sourceType.GetSecurityNodeInterfaces().Any();
    }

    public static IEnumerable<Type> GetSecurityNodeInterfaces(this Type sourceType)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

        return sourceType.GetAllInterfaces().Where(i => (i.IsGenericType ? i.GetGenericTypeDefinition() : i).HasAttribute<SecurityNodeAttribute>() || i == typeof(ISecurityContext));
    }

    public static IEnumerable<Type> GetGenericSecurityNodeInterfaces(this Type sourceType)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

        return sourceType.GetSecurityNodeInterfaces().Where(interfaceType => interfaceType.IsGenericType);
    }
}
