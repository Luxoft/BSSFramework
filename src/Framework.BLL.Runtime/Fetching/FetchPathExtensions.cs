using System.Collections.ObjectModel;
using System.Reflection;

using Framework.BLL.Domain.Fetching;
using Framework.BLL.Domain.Persistent.Extensions;
using Framework.Core;


namespace Framework.BLL.Fetching;

internal static class PropertyPathExtensions
{
    public static ReadOnlyCollection<PropertyPath> GetFetchPaths(this PropertyInfo property)
    {
        return property.GetPropertyPaths<FetchPathAttribute>();
    }
}
