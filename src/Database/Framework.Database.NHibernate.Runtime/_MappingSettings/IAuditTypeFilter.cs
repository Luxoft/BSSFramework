using System.Reflection;

namespace Framework.Database.NHibernate._MappingSettings;

public interface IAuditTypeFilter
{
    bool IsAuditedType(Type type);

    bool IsAuditedProperty(Type type, PropertyInfo propertyInfo);
}
