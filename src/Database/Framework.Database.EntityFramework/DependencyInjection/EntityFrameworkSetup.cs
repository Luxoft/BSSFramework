using CommonFramework.DependencyInjection;

using Framework.Database.EntityFramework.Sessions;
using Framework.DependencyInjection;

using GenericQueryable.DependencyInjection;
using GenericQueryable.EntityFramework;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.DependencyInjection;

public class EntityFrameworkSetup : IEntityFrameworkSetup, IServiceInitializer
{
    public void Initialize(IServiceCollection services)
    {
        //services.AddSingleton<IAuditRevisionUserAuthenticationService, AuditRevisionUserAuthenticationService>();

        services.AddScoped(typeof(IAsyncDal<,>), typeof(EfAsyncDal<,>));

        services.AddGenericQueryable(v => v.SetFetchService<EfFetchService>().SetTargetMethodExtractor<EfTargetMethodExtractor>());

        //For close db session by middleware
        services.AddScopedFromLazyObject<IEfSession, EfSession>();
        services.AddScopedFrom<ILazyObject<IDBSession>, ILazyObject<IEfSession>>();

        //services.AddSingleton<IEfSessionEnvironmentSettings, EfSessionEnvironmentSettings>();

        //services.AddSingleton<IDefaultConnectionStringSource, DefaultConnectionStringSource>();
    }
}
