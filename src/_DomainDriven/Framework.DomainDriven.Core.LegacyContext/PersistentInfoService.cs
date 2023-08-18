using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL.Tracking;
using Framework.Persistent.Mapping;

namespace Framework.DomainDriven;

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
