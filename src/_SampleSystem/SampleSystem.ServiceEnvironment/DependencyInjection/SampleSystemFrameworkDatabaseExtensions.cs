using Framework.Authorization.Generated.DAL.NHibernate;
using Framework.Configuration.Generated.DAL.NHibernate;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.ServiceModel.IAD;
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
        return settings.AddDatabaseSettings(
            setupObj => setupObj.AddEventListener<DefaultDBSessionEventListener>()

                                .AddMapping(new AuthorizationMappingSettings())
                                .AddMapping(new ConfigurationMappingSettings())

                                .Pipe(
                                    includeTypedAudit,
                                    s => s

                                         .AddMapping(new SampleSystemSystemAuditMappingSettings(string.Empty))
                                         .AddMapping(new SampleSystemSystemRevisionAuditMappingSettings(string.Empty)))

                                .AddMapping(new SampleSystemMappingSettings(new DatabaseName(string.Empty, "app"))));
    }

    public static IBssFrameworkSettings AddDatabaseVisitors(this IBssFrameworkSettings settings)
    {
        return settings.AddDatabaseVisitors<ExpressionVisitorContainerDomainIdentItem<PersistentDomainObjectBase, Guid>>();
    }
}
