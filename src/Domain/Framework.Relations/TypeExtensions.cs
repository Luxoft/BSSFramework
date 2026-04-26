using System.Reflection;

using Anch.Core;

using Framework.Core;

namespace Framework.Relations;

public static class TypeExtensions
{
    public static IEnumerable<Type> GetDetailTypes(this Type type) =>
        type.GetInterfaces()
            .Select(i => i.GetInterfaceImplementationArgument(typeof(IMaster<>)))
            .Where(detailType => detailType != null)
            .Select(v => v!);

    public static IEnumerable<PropertyInfo> GetDetailProperties(this Type type) =>
        from detailType in type.GetDetailTypes()

        join property in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) on detailType equals property.PropertyType
            .GetCollectionElementType() into propertyGroup

        select propertyGroup.Count() == 1 ? propertyGroup.Single() : propertyGroup.Single(property => property.HasPrivateField());

}
