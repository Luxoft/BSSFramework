using System.Reflection;

namespace Framework.DomainDriven._Visitors;

public interface IIdPropertyResolver
{
    PropertyInfo Resolve(Type persistentDomainObjectBase);
}
