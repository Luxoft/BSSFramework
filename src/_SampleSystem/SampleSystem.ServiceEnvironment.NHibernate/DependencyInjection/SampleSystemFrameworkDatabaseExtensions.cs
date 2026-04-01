using CommonFramework;

using Framework.Authorization.Generated.DAL.NHibernate;
using Framework.Configuration.Generated.DAL.NHibernate;
using Framework.Database;
using Framework.Database.NHibernate;
using Framework.Database.NHibernate.DependencyInjection;
using Framework.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.AuditDAL.NHibernate;
using SampleSystem.Generated.DAL.NHibernate;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public class SampleSystemNHibernateExtension(bool includeTypedAudit) : IBssFrameworkExtension
{
    public SampleSystemNHibernateExtension()
        : this(true)
    {
    }

    public void AddServices(IServiceCollection services)
    {
        var appDatabase = new DatabaseName(string.Empty, "app");
        var appAuditDatabase = new DatabaseName(string.Empty, "appAudit");

        services.AddNHibernate(
                    setupObj => setupObj.AddLegacyDatabaseSettings()
                                        .AddMapping(new AuthorizationMappingSettings())
                                        .AddMapping(new ConfigurationMappingSettings())
                                        .Pipe(
                                            includeTypedAudit,
                                            s => s

                                                 .AddMapping(new SampleSystemSystemAuditMappingSettings(appAuditDatabase))
                                                 .AddMapping(new SampleSystemSystemRevisionAuditMappingSettings(appAuditDatabase))
                                                 .AddMapping(new SampleSystemMappingSettings(appDatabase))));
    }
}
