using System.Reflection;

using Framework.Core;
using Framework.Database.Mapping;
using Framework.Database.Mapping.Extensions;

namespace Framework.Database;

public class PersistentInfoService : IPersistentInfoService
{
    public bool IsPersistent(Type type) => !type.HasAttribute<NotPersistentClassAttribute>();

    public bool IsPersistent(PropertyInfo propertyInfo) => propertyInfo.IsPersistent();
}
