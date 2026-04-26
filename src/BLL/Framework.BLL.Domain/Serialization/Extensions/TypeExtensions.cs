using System.Reflection;

using Anch.Core;

using Framework.BLL.Domain.Extensions;

namespace Framework.BLL.Domain.Serialization.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<PropertyInfo> GetSerializationProperties(this Type type, DTORole role = DTORole.Client)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return from property in type.IsInterface ? type.GetAllInterfaceProperties() : type.GetProperties(BindingFlags.Public | BindingFlags.Instance)

               where !property.IsIgnored(role) && !property.GetIndexParameters().Any()

               select property;
    }

    public static bool HasVisualIdentityProperties(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetSerializationProperties().Any(prop => prop.IsVisualIdentity());
    }
}
