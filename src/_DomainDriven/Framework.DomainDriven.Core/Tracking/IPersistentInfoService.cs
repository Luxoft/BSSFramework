using System.Reflection;

namespace Framework.DomainDriven.BLL.Tracking;

public interface IPersistentInfoService
{
    bool IsPersistent(Type type);

    bool IsPersistent(PropertyInfo pePropertyInfo);
}
