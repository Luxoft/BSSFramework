using System.Data;

using CommonFramework.DependencyInjection;

using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.DALExceptions;
using Framework.DomainDriven.NHibernate.Audit;

using GenericQueryable;

using Microsoft.Extensions.DependencyInjection;

using NHibernate;

namespace Framework.DomainDriven.NHibernate;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddNHibernate(this IServiceCollection services, Action<INHibernateSetupObject> setupAction)
    {
        var setupObject = new NHibernateSetupObject();

        services.AddSingleton<IAuditRevisionUserAuthenticationService, AuditRevisionUserAuthenticationService>();

        services.AddScoped(typeof(IAsyncDal<,>), typeof(NHibAsyncDal<,>));

        services.AddScoped<IDBSessionSettings, DBSessionSettings>();

        services.AddSingleton(typeof(INHibFetchService<>), typeof(NHibFetchService<>));
        services.AddSingleton<IGenericQueryableExecutor, NHibGenericQueryableExecutor>();

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

        setupAction(setupObject);

        setupObject.Initialize(services);

        return services;
    }
}
