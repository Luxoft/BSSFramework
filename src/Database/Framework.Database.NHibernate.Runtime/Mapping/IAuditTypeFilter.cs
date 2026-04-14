using System.Reflection;

namespace Framework.Database.NHibernate.Mapping;

public interface IAuditTypeFilter
{
    bool IsAuditedType(Type type);

    bool IsAuditedProperty(Type type, PropertyInfo propertyInfo);
}
