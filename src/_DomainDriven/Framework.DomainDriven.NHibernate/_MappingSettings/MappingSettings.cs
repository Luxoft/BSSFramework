#nullable enable

using System.Reflection;
using System.Xml.Linq;

using Framework.Persistent;

namespace Framework.DomainDriven.NHibernate;

public class MappingSettings<TPersistentDomainObjectBase> : MappingSettings
{
    public MappingSettings(Assembly mappingAssembly, IEnumerable<Type>? types = null)
            : this(mappingAssembly, new DatabaseName(typeof(TPersistentDomainObjectBase).ExtractSystemName()), types)
    {
    }

    public MappingSettings(Assembly mappingAssembly, DatabaseName databaseName, IEnumerable<Type>? types = null)
            : this(mappingAssembly, databaseName, null, types)
    {
    }

    public MappingSettings(Assembly mappingAssembly, DatabaseName databaseName, AuditDatabaseName? auditDatabaseName, IEnumerable<Type>? types = null)
            : base(typeof(TPersistentDomainObjectBase), mappingAssembly, databaseName, auditDatabaseName, types)
    {
    }

    public MappingSettings(IEnumerable<XDocument> mappingXmls, DatabaseName databaseName, bool auditDatabaseName, IEnumerable<Type>? types = null)
            : this(mappingXmls, databaseName, auditDatabaseName ? databaseName.ToDefaultAudit() : null, types)
    {
    }

    public MappingSettings(IEnumerable<XDocument> mappingXmls, DatabaseName databaseName, AuditDatabaseName? auditDatabaseName, IEnumerable<Type>? types = null)
            : base(typeof(TPersistentDomainObjectBase), mappingXmls, databaseName, auditDatabaseName, types)
    {
    }

    public MappingSettings(IEnumerable<XDocument> mappingXmls, bool isAudit)
            : this(mappingXmls, new DatabaseName(typeof(TPersistentDomainObjectBase).ExtractSystemName()),
                   isAudit ? new DatabaseName(typeof(TPersistentDomainObjectBase).ExtractSystemName()).ToDefaultAudit() : null)
    {
    }

    public MappingSettings(IConfigurationInitializer initializer, DatabaseName databaseName, bool isAudit, IEnumerable<Type>? types = null)
            : this(initializer, databaseName, isAudit ? databaseName.ToDefaultAudit() : null, types)
    {
    }

    public MappingSettings(IConfigurationInitializer initializer, DatabaseName databaseName, AuditDatabaseName? auditDatabaseName, IEnumerable<Type>? types = null)
            : base(typeof(TPersistentDomainObjectBase), initializer, databaseName, auditDatabaseName, types)
    {
    }
}
