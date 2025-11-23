using System.Data;

using CommonFramework.DependencyInjection;

using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.DALExceptions;
using Framework.DomainDriven.NHibernate.Audit;

using GenericQueryable.Fetching;
using GenericQueryable.Services;

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

        
        services.AddSingleton<IGenericQueryableExecutor, GenericQueryableExecutor>();
        services.AddSingleton<IMethodRedirector, MethodRedirector>();

        services.AddSingleton(typeof(INHibRawFetchService<>), typeof(NHibRawFetchService<>));
        services.AddSingleton(typeof(IFetchService), typeof(NHibFetchService));
        services.AddSingleton<ITargetMethodExtractor, NhibTargetMethodExtractor>();

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
