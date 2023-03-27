using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Persistent;

public static class TypeExtensions
{
    public static bool IsIdentity(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetIdentType() != null;
    }

    public static Type GetIdentType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var request = from i in type.GetInterfaces()

                      where i.IsGenericTypeImplementation(typeof(IIdentityObject<>))

                      select i.GetGenericArguments().Single(() => new Exception($"Type:{type.Name} has more then one generic argument"));

        return request.SingleOrDefault(() => new System.ArgumentException($"Type:{type.Name} has more then one IIdentityObject interface"));
    }

    [Obsolete]
    public static Type GetIdentityType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetInterfaces().SingleOrDefault(i => i.IsGenericTypeImplementation(typeof(IIdentityObject<>)));
    }

    [Obsolete("If you use this method contact negorov or someone else from IaD Framework team. PersistentDomainObjectAttribute is going to be removed in future version", false)]
    public static Type GetPersistentDomainObjectType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetCustomAttribute<PersistentDomainObjectAttribute>()
                   .FromMaybe(() => $"{typeof(PersistentDomainObjectAttribute).Name} not found")
                   .Type;
    }

    public static bool IsIdentityContainerType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.IsInterfaceImplementation(typeof(IIdentityObjectContainer<>));
    }

    public static bool IsVisualIdentityObject(this Type type, bool publicImplement = true)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.IsPropertyImplement(typeof(IVisualIdentityObject));
    }


    public static IEnumerable<Type> GetDetailTypes(this Type type)
    {
        return type.GetInterfaces()
                   .Select(i => i.GetInterfaceImplementationArgument(typeof(IMaster<>)))
                   .Where(detailType => detailType != null);
    }

    public static IEnumerable<PropertyInfo> GetDetailProperties(this Type type)
    {
        return from detailType in type.GetDetailTypes()

               join property in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) on detailType equals property.PropertyType.GetCollectionElementType() into propertyGroup

               select propertyGroup.Count() == 1 ? propertyGroup.Single() : propertyGroup.Single(property => property.HasPrivateField());
    }

    public static Guid GetDomainTypeId([NotNull] this Type type, bool throwIfDefault = false)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var domainTypeAttr = type.GetCustomAttribute<DomainTypeAttribute>();

        if (domainTypeAttr != null)
        {
            return domainTypeAttr.Id;
        }
        else if (type == typeof(string))
        {
            return PersistentHelper.StringDomainTypeId;
        }
        else if (type == typeof(bool))
        {
            return PersistentHelper.BooleanDomainTypeId;
        }
        else if (type == typeof(Guid))
        {
            return PersistentHelper.GuidDomainTypeId;
        }
        else if (type == typeof(int))
        {
            return PersistentHelper.Int32DomainTypeId;
        }
        else if (type == typeof(DateTime))
        {
            return PersistentHelper.DateTimeTypeId;
        }
        else if (type == typeof(decimal))
        {
            return PersistentHelper.DecimalTypeId;
        }
        else if (throwIfDefault)
        {
            throw new ArgumentOutOfRangeException(nameof(type), $"id for type \"{type.FullName}\" not initialized");
        }
        else
        {
            return Guid.Empty;
        }
    }

    public static Guid GetTargetSystemId([NotNull] this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.Assembly == typeof(void).Assembly
                       ? PersistentHelper.BaseTargetSystemId
                       : type.GetCustomAttribute<TargetSystemAttribute>().Maybe(attr => attr.Id);
    }

    public static string GetTargetSystemName([NotNull] this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.Assembly == typeof(void).Assembly
                       ? PersistentHelper.BaseTargetSystemName
                       : type.GetCustomAttribute<TargetSystemAttribute>().Maybe(attr => attr.Name) ?? type.Namespace.Split('.').Skip("Framework", false).First();
    }

    internal static bool SafeEquals(this Type t1, Type t2)
    {
        if (t1 == null) throw new ArgumentNullException(nameof(t1));
        if (t2 == null) throw new ArgumentNullException(nameof(t2));

        return t1.IsGenericTypeDefinition == t2.IsGenericTypeDefinition && t1 == t2;
    }

    internal static bool SafeIsAssignableFrom(this Type t1, Type t2)
    {
        if (t1 == null) throw new ArgumentNullException(nameof(t1));
        if (t2 == null) throw new ArgumentNullException(nameof(t2));

        return t1.SafeEquals(t2) || t1.IsAssignableFrom(t2);
    }
}
