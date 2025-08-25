using System.Reflection;

namespace Framework.Core;

public static class PropertyPathExtensions
{
    public static PropertyPath ToPropertyPath(this IEnumerable<PropertyInfo> properties)
    {
        if (properties == null) throw new ArgumentNullException(nameof(properties));

        return new PropertyPath(properties);
    }

}
