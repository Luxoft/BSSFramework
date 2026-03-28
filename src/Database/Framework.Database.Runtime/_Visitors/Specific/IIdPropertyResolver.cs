using System.Reflection;

namespace Framework.Database._Visitors.Specific;

public interface IIdPropertyResolver
{
    PropertyInfo Resolve(Type persistentDomainObjectBase);
}
