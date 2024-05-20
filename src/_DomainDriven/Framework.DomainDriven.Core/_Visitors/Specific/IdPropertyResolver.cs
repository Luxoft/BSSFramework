using System.Reflection;

using Framework.Core;

namespace Framework.DomainDriven._Visitors;

public class IdPropertyResolver : IIdPropertyResolver
{
    public PropertyInfo Resolve(Type persistentDomainObjectBase)
    {
        return persistentDomainObjectBase.GetProperty("Id", () => new Exception("Id property not found"));
    }
}
