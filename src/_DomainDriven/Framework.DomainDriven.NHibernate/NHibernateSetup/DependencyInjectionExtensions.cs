using System.Data;

using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.DALExceptions;

using Microsoft.Extensions.DependencyInjection;

using NHibernate;

namespace Framework.DomainDriven.NHibernate;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDatabaseSettings(this IServiceCollection services, Action<INHibernateSetupObject> setup)
    {
        var setupObject = new NHibernateSetupObject();

        services.AddScoped<INHibSessionSetup, NHibSessionSettings>();

        //For close db session by middleware
        services.AddScopedFromLazyObject<INHibSession, NHibSession>();
        services.AddScopedFrom<ILazyObject<IDBSession>, ILazyObject<INHibSession>>();
        services.AddScopedFrom((ILazyObject<IDBSession> lazyDbSession) => lazyDbSession.Value);
        services.AddScoped<IDBSessionManager, DBSessionManager>();

        services.AddScopedFrom<ISession, INHibSession>(session => session.NativeSession);
        services.AddScopedFrom<IDbTransaction, IDBSession>(session => session.Transaction);

        services.AddSingleton<INHibSessionEnvironmentSettings, NHibSessionEnvironmentSettings>();

        services.AddSingleton<IDalValidationIdentitySource, DalValidationIdentitySource>();

        services.AddSingleton<NHibSessionEnvironment>();

        services.AddSingleton<IDefaultConnectionStringSource, DefaultConnectionStringSource>();

        setup(setupObject);

        setupObject.Initialize(services);

        return services;
    }
}
