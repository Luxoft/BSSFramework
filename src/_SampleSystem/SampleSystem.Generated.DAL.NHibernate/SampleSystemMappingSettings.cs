using System.Xml.Linq;

using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using SampleSystem.Domain;

namespace SampleSystem.Generated.DAL.NHibernate;

public record SampleSystemMappingSettings : MappingSettings<PersistentDomainObjectBase>
{
    public SampleSystemMappingSettings(DatabaseName databaseName)
        : base(typeof(SampleSystemMappingSettings).Assembly, databaseName, databaseName.ToDefaultAudit())
    {
    }

    /// <summary>
    /// For DBGenerator
    /// </summary>
    public SampleSystemMappingSettings(
        IEnumerable<XDocument> mappingXmls,
        DatabaseName databaseName,
        AuditDatabaseName auditDatabaseName,
        IEnumerable<Type> types = null)
        : base(mappingXmls, databaseName, auditDatabaseName, types)
    {
    }
}
