using System.Reflection;

using Framework.Core;

namespace Framework.Database._Visitors.Specific;

public class IdPropertyResolver : IIdPropertyResolver
{
    public PropertyInfo Resolve(Type persistentDomainObjectBase) => persistentDomainObjectBase.GetProperty("Id", () => new Exception("Id property not found"))!;
}
