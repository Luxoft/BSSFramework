using System;
using System.Reflection;

namespace Framework.DomainDriven.NHibernate;

public interface IAuditTypeFilter
{
    bool IsAuditedType(Type type);

    bool IsAuditedProperty(Type type, PropertyInfo propertyInfo);
}
