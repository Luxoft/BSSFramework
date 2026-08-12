using System.Collections.Immutable;
using System.Reflection;

using Framework.BLL.Domain.Persistent.Attributes;
using Framework.Core;

namespace Framework.BLL.Services;

public interface IPropertyPathService
{
    PropertyPath? TryGetPropertyPath<TAttribute>(PropertyInfo property, bool recurse)
        where TAttribute : Attribute, IPathAttribute;

    ImmutableArray<PropertyPath> GetPropertyPaths<TAttribute>(PropertyInfo property)
        where TAttribute : Attribute, IPathAttribute;

    PropertyPath? TryGetExpandPath(PropertyInfo property);

    PropertyPath GetExpandPathOrSelf(PropertyInfo property);

    PropertyPath WithExpand(PropertyPath propertyPath);
}
