using System;
using System.Reflection;

namespace Framework.DomainDriven;

public interface IIdPropertyResolver
{
    PropertyInfo Resolve(Type persistentDomainObjectBase);
}
