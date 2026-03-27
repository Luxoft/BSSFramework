using System.Reflection;

namespace Framework.Tracking;

public interface IPersistentInfoService
{
    bool IsPersistent(Type type);

    bool IsPersistent(PropertyInfo propertyInfo);
}
