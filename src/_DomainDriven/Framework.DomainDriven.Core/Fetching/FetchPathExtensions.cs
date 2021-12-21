using System.Collections.ObjectModel;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven
{
    internal static class PropertyPathExtensions
    {
        public static ReadOnlyCollection<PropertyPath> GetFetchPaths(this PropertyInfo property)
        {
            return property.GetPropertyPaths<FetchPathAttribute>();
        }
    }
}