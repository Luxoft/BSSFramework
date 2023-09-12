using System.Collections.ObjectModel;

using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Security;

public static class TypeExtensions
{
    internal static SecurityOperation GetSecurityOperation(this Type securityOperationType, string name)
    {
        return (SecurityOperation)securityOperationType.GetProperty(name)!.GetValue(null);
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

    public static IEnumerable<Enum> GetSecurityOperationCodes(this Type enumType)
    {
        if (enumType == null) throw new ArgumentNullException(nameof(enumType));

        return from Enum securityOperationCode in Enum.GetValues(enumType)

               //where !securityOperationCode.IsDefaultEnumValue()

               select securityOperationCode;
    }

    public static Dictionary<Type, ReadOnlyCollection<SecurityOperation>> GetTypesWithSecondarySecurityOperations(this IEnumerable<Type> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var request = from type in source

                      let attr = type.GetViewDomainObjectAttribute()

                      where attr != null && attr.SecondaryOperations.Any()

                      select new
                             {
                                     Type = type,

                                     Operations = attr.AllOperations.ToReadOnlyCollection()
                             };

        return request.ToDictionary(pair => pair.Type, pair => pair.Operations);
    }
}
