using CommonFramework;
using CommonFramework.Auth;

using Framework.Database.NHibernate._MappingSettings;
using Framework.Database.NHibernate.Audit;
using Framework.Database.NHibernate.DAL.Revisions;
using Framework.Database.NHibernate.Envers.Forke;

using NHibernate.Cfg;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Configuration.Store;
using NHibernate.Envers.Strategy;

namespace Framework.Database.NHibernate;

public static class AuditedExtension
{
    public static void InitializeAudit(this Configuration configuration, IEnumerable<MappingSettings> preMappingSettings, ICurrentUser defaultCurrentUser)
    {
        var mappingSettings = preMappingSettings.Where(z => z.IsAudited()).ToArray();

        if (!mappingSettings.Any())
        {
            return;
        }

        var tablePostfix = "Audit";

        configuration.SetEnversProperty(ConfigurationKey.AuditStrategy, typeof(DefaultAuditStrategy));
        configuration.SetEnversProperty(ConfigurationKey.TrackEntitiesChangedInRevision, false);
        ////configuration.SetEnversProperty(ConfigurationKey.RevisionOnCollectionChange, true);
        configuration.SetEnversProperty(ConfigurationKey.GlobalWithModifiedFlag, true);
        configuration.SetEnversProperty(ConfigurationKey.StoreDataAtDelete, false);

        configuration.SetEnversProperty(ConfigurationKey.AuditTableSuffix, tablePostfix);

        var auditedAssemblies = mappingSettings.GroupBy(z => z.PersistentDomainObjectBaseType);

        var provider = GetProvider(auditedAssemblies, tablePostfix, defaultCurrentUser);

        configuration.IntegrateWithEnvers(new AuditEventListenerForke(), provider);
    }

    private static IMetaDataProvider GetProvider<T>(
            IEnumerable<IGrouping<T, MappingSettings>> auditedAssemblies,
            string tablePostfix,
            ICurrentUser defaultCurrentUser)
    {
        var assemblies = auditedAssemblies.SelectMany(z => z).ToArray();

        var auditSchema = assemblies.Select(z => z.AuditDatabase.RevisionEntitySchema).Distinct().SingleOrDefault(() => new ArgumentException("More then one AuditDatabase. There can be only one!"));

        return new AuditMetadataProvider<AuditRevisionEntity>(assemblies, auditSchema, tablePostfix, new AuditRevisionEntityListener<AuditRevisionEntity>(defaultCurrentUser));
    }
}
