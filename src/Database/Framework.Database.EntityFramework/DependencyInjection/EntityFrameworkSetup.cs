using Anch.DependencyInjection;
using Anch.GenericQueryable.DependencyInjection;
using Anch.GenericQueryable.EntityFramework;

using Framework.Core;
using Framework.Database.EntityFramework.Sessions;
using Framework.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.DependencyInjection;

public class EntityFrameworkSetup : IEntityFrameworkSetup, IServiceInitializer
{
    private readonly List<IEntityFrameworkSetupExtension> extensions = new List<IEntityFrameworkSetupExtension>();

    private Action<IServiceCollection>? dbContextInit;

    public void Initialize(IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncDal<,>), typeof(EfAsyncDal<,>));

        services.AddGenericQueryable(v => v.SetFetchService<EfFetchService>().SetTargetMethodExtractor<EfTargetMethodExtractor>());

        //For close db session by middleware
        services.AddScopedFromLazyObject<IEfSession, EfSession>();
        services.AddScopedFrom<ILazyObject<IDBSession>, ILazyObject<IEfSession>>();

        //services.AddSingleton<IDefaultConnectionStringSource, DefaultConnectionStringSource>();

        (this.dbContextInit ?? throw new InvalidOperationException("DbContext has not been initialized.")).Invoke(services);

        this.extensions.ForEach(ex => ex.AddServices(services));
    }

    public IEntityFrameworkSetup SetDbContext<TDbContext>()
        where TDbContext : DbContext
    {
        this.dbContextInit = sc =>
        {
            sc.AddScopedFrom<DbContext, TDbContext>();
        };

        return this;
    }

    public IEntityFrameworkSetup AddExtension(IEntityFrameworkSetupExtension extension)
    {
        this.extensions.Add(extension);

        return this;
    }
}
