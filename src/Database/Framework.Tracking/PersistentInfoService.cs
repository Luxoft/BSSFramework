using System.Reflection;

using Framework.Core;
using Framework.Database.Mapping;
using Framework.Database.Mapping.Extensions;

namespace Framework.Tracking;

public class PersistentInfoService : IPersistentInfoService
{
    public bool IsPersistent(Type type)
    {
        return !type.HasAttribute<NotPersistentClassAttribute>();
    }

    public bool IsPersistent(PropertyInfo propertyInfo)
    {
        return propertyInfo.IsPersistent();
    }
}
