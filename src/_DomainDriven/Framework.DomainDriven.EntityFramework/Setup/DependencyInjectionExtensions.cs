using System.Data;

using CommonFramework.DependencyInjection;

using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.DALExceptions;

using GenericQueryable.EntityFramework;
using GenericQueryable.Fetching;
using GenericQueryable.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.EntityFramework;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, Action<IEntityFrameworkSetupObject> setupAction)
    {
        var setupObject = new EntityFrameworkSetupObject();

        //services.AddSingleton<IAuditRevisionUserAuthenticationService, AuditRevisionUserAuthenticationService>();

        services.AddScoped(typeof(IAsyncDal<,>), typeof(EfAsyncDal<,>));

        services.AddScoped<IDBSessionSettings, DBSessionSettings>();


        services.AddSingleton<IMethodRedirector, MethodRedirector>();
        services.AddSingleton<IGenericQueryableExecutor, GenericQueryableExecutor>();

        services.AddSingleton<IFetchService, EfFetchService>();
        services.AddSingleton<ITargetMethodExtractor, EfTargetMethodExtractor>();

        //For close db session by middleware
        services.AddScopedFromLazyObject<IEfSession, EfSession>();
        services.AddScopedFrom<ILazyObject<IDBSession>, ILazyObject<IEfSession>>();
        services.AddScopedFrom((ILazyObject<IDBSession> lazyDbSession) => lazyDbSession.Value);
        services.AddScoped<IDBSessionManager, DBSessionManager>();

        //services.AddScopedFrom<DbContext, IEfSession>(session => session.NativeSession);
        services.AddScopedFrom<IDbTransaction, IDBSession>(session => session.Transaction);

        //services.AddSingleton<IEfSessionEnvironmentSettings, EfSessionEnvironmentSettings>();

        services.AddSingleton<IDalValidationIdentitySource, DalValidationIdentitySource>();

        //services.AddSingleton<IDefaultConnectionStringSource, DefaultConnectionStringSource>();

        setupAction(setupObject);

        //setupObject.Initialize(services);

        return services;
    }
}
