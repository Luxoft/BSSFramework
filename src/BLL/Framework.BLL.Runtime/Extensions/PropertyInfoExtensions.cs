using System.Reflection;

using Anch.Core;

namespace Framework.BLL.Extensions;

public static class PropertyInfoExtensions
{
    extension(PropertyInfo property)
    {
        public Type GetNestedType() => property.PropertyType.GetCollectionElementType() ?? property.PropertyType;
    }
}
