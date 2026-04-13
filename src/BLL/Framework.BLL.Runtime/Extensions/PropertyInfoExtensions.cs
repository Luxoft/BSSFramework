using System.Reflection;

using CommonFramework;

namespace Framework.BLL.Extensions;

public static class PropertyInfoExtensions
{
    extension(PropertyInfo property)
    {
        public Type GetNestedType() => property.PropertyType.GetCollectionElementType() ?? property.PropertyType;
    }
}
