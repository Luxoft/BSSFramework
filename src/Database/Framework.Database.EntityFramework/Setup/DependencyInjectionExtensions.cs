using CommonFramework.DependencyInjection;

using Framework.Core.LazyObject;
using Framework.Database.EntityFramework.Sessions;
using Framework.DependencyInjection;

using GenericQueryable.DependencyInjection;
using GenericQueryable.EntityFramework;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.Setup;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, Action<IEntityFrameworkSetupObject> setupAction)
    {
        var setupObject = new EntityFrameworkSetupObject();

        //services.AddSingleton<IAuditRevisionUserAuthenticationService, AuditRevisionUserAuthenticationService>();

        services.AddScoped(typeof(IAsyncDal<,>), typeof(EfAsyncDal<,>));

        services.AddScoped<DBSessionSettings>();

        services.AddGenericQueryable(v => v.SetFetchService<EfFetchService>().SetTargetMethodExtractor<EfTargetMethodExtractor>());

        //For close db session by middleware
        services.AddScopedFromLazyObject<IEfSession, EfSession>();
        services.AddScopedFrom<ILazyObject<IDBSession>, ILazyObject<IEfSession>>();
        services.AddScopedFrom((ILazyObject<IDBSession> lazyDbSession) => lazyDbSession.Value);
        services.AddScoped<IDBSessionManager, DBSessionManager>();

        //services.AddSingleton<IEfSessionEnvironmentSettings, EfSessionEnvironmentSettings>();

        //services.AddSingleton<IDefaultConnectionStringSource, DefaultConnectionStringSource>();

        setupAction(setupObject);

        //setupObject.Initialize(services);

        return services;
    }
}
