using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.NHibernate.Audit;

using JetBrains.Annotations;

using NHibernate.Cfg;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Configuration.Store;
using NHibernate.Envers.Patch.Forke;
using NHibernate.Envers.Strategy;

namespace Framework.DomainDriven.NHibernate;

public static class AuditedExtension
{
    public static void InitializeAudit([NotNull] this Configuration configuration, [NotNull] IEnumerable<IMappingSettings> preMappingSettings, [NotNull] IAuditRevisionUserAuthenticationService auditRevisionUserAuthenticationService)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (preMappingSettings == null)
        {
            throw new ArgumentNullException(nameof(preMappingSettings));
        }

        if (auditRevisionUserAuthenticationService == null)
        {
            throw new ArgumentNullException(nameof(auditRevisionUserAuthenticationService));
        }

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

        var provider = GetProvider(auditedAssemblies, tablePostfix, auditRevisionUserAuthenticationService);

        configuration.IntegrateWithEnvers(new AuditEventListenerForke(), provider);
    }

    private static IMetaDataProvider GetProvider<T>(
            IEnumerable<IGrouping<T, IMappingSettings>> auditedAssemblies,
            string tablePostfix,
            IAuditRevisionUserAuthenticationService auditRevisionUserAuthenticationService)
    {
        var assemblies = auditedAssemblies.SelectMany(z => z).ToArray();

        var auditesSchemas = assemblies.Select(z => z.AuditDatabase.RevisionEntitySchema).Distinct().SingleOrDefault(() => new ArgumentException("More then one AuditDatabase. There can be only one!"));

        return new AuditMetadataProvider<AuditRevisionEntity>(assemblies, auditesSchemas, tablePostfix, new AuditRevisionEntityListener<AuditRevisionEntity>(auditRevisionUserAuthenticationService));
    }
}
