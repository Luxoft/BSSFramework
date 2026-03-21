using System.Reflection;

namespace Framework.Application._Visitors.Specific;

public interface IIdPropertyResolver
{
    PropertyInfo Resolve(Type persistentDomainObjectBase);
}
