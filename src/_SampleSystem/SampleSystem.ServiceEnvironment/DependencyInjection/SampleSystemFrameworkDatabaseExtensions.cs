using Framework.Authorization.Generated.DAL.NHibernate;
using Framework.Configuration.Generated.DAL.NHibernate;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.Setup;

using Microsoft.Extensions.Configuration;

using SampleSystem.AuditDAL.NHibernate;
using SampleSystem.Domain;
using SampleSystem.Generated.DAL.NHibernate;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemFrameworkDatabaseExtensions
{
    public static IBssFrameworkSettings AddDatabaseSettings(this IBssFrameworkSettings services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        return services.AddDatabaseSettings(connectionString);
    }

    public static IBssFrameworkSettings AddDatabaseSettings(
        this IBssFrameworkSettings services,
        string connectionString,
        bool includeTypedAudit = true)
    {
        return services.AddDatabaseSettings(
            setupObj => setupObj.AddEventListener<DefaultDBSessionEventListener>()

                                .AddMapping(AuthorizationMappingSettings.CreateDefaultAudit(string.Empty))
                                .AddMapping(ConfigurationMappingSettings.CreateDefaultAudit(string.Empty))

                                .Pipe(
                                    includeTypedAudit,
                                    s => s

                                         .AddMapping(new SampleSystemSystemAuditMappingSettings(string.Empty))
                                         .AddMapping(new SampleSystemSystemRevisionAuditMappingSettings(string.Empty)))


                                .AddMapping(new SampleSystemMappingSettings(new DatabaseName(string.Empty, "app"), connectionString)));
    }

    public static IBssFrameworkSettings AddDatabaseVisitors(this IBssFrameworkSettings services)
    {
        return services.AddDatabaseVisitors<ExpressionVisitorContainerDomainIdentItem<PersistentDomainObjectBase, Guid>>();
    }
}
