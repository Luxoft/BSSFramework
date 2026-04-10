using System.Reflection;

namespace Framework.ExtendedMetadata;

public static class PropertyInfoExtensions
{
    public static PropertyInfo GetUnderlyingSystemProperty(this PropertyInfo property)
    {
        var realType = property.DeclaringType!.UnderlyingSystemType;

        return realType.GetProperty(
                   property.Name,
                   property.PropertyType.UnderlyingSystemType)
               ?? throw new Exception("Base property not found");
    }
}
