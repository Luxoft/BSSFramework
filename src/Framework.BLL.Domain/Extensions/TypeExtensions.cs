using System.Reflection;
using CommonFramework;

using Framework.Application.Domain;
using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.MasterDetails;
using Framework.Core;

using SecuritySystem;

namespace Framework.BLL.Domain.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<Type> GetDetailTypes(this Type type)
    {
        return type.GetInterfaces()
                   .Select(i => i.GetInterfaceImplementationArgument(typeof(IMaster<>)))
                   .Where(detailType => detailType != null)
                   .Select(v => v!);
    }

    public static IEnumerable<PropertyInfo> GetDetailProperties(this Type type)
    {
        return from detailType in type.GetDetailTypes()

               join property in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) on detailType equals property.PropertyType.GetCollectionElementType() into propertyGroup

               select propertyGroup.Count() == 1 ? propertyGroup.Single() : propertyGroup.Single(property => property.HasPrivateField());
    }

    public static Type? GetIdentType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var request = from i in type.GetInterfaces()

                      where i.IsGenericTypeImplementation(typeof(IIdentityObject<>))

                      select i.GetGenericArguments().Single(() => new Exception($"Type:{type.Name} has more then one generic argument"));

        return request.SingleOrDefault(() => new ArgumentException($"Type:{type.Name} has more then one IIdentityObject interface"));
    }

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
