using Framework.Authorization.Generated.DAL.NHibernate;
using Framework.Configuration.Generated.DAL.NHibernate;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.Setup;

using SampleSystem.AuditDAL.NHibernate;
using SampleSystem.Domain;
using SampleSystem.Generated.DAL.NHibernate;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemFrameworkDatabaseExtensions
{
    public static IBssFrameworkSettings AddDatabaseSettings(
        this IBssFrameworkSettings settings,
        bool includeTypedAudit = true)
    {
        var appDatabase = new DatabaseName(string.Empty, "app");
        var appAuditDatabase = new DatabaseName(string.Empty, "appAudit");

        return settings.AddDatabaseSettings(
            setupObj => setupObj.AddMapping(new AuthorizationMappingSettings())
                                .AddMapping(new ConfigurationMappingSettings())

                                .Pipe(
                                    includeTypedAudit,
                                    s => s

                                         .AddMapping(new SampleSystemSystemAuditMappingSettings(appAuditDatabase))
                                         .AddMapping(new SampleSystemSystemRevisionAuditMappingSettings(appAuditDatabase))
                                         .AddMapping(new SampleSystemMappingSettings(appDatabase))));
    }

    public static IBssFrameworkSettings AddDatabaseVisitors(this IBssFrameworkSettings settings)
    {
        return settings.AddDatabaseVisitors<ExpressionVisitorContainerDomainIdentItem<PersistentDomainObjectBase, Guid>>();
    }
}
