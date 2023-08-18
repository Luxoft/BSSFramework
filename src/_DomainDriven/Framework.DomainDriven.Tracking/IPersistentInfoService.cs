using System.Reflection;

namespace Framework.DomainDriven.Tracking;

public interface IPersistentInfoService
{
    bool IsPersistent(Type type);

    bool IsPersistent(PropertyInfo propertyInfo);
}
