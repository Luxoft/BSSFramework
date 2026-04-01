using System.Xml.Linq;

using Framework.Database;
using Framework.Database.NHibernate._MappingSettings;

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
        AuditDatabaseName auditDatabaseName)
        : base(mappingXmls, databaseName, auditDatabaseName)
    {
    }
}
