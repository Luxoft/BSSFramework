using Anch.DependencyInjection;
using Anch.GenericQueryable.DependencyInjection;
using Anch.GenericQueryable.EntityFramework;

using Framework.Database.EntityFramework.Sessions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.DependencyInjection;

public class EntityFrameworkSetup : IEntityFrameworkSetup, IServiceInitializer
{
    private readonly List<IEntityFrameworkSetupExtension> extensions = [];

    private Action<IServiceCollection>? dbContextInit;

    public void Initialize(IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncDal<,>), typeof(EfAsyncDal<,>));

        services.AddGenericQueryable(v => v.SetFetchService<EfFetchService>().SetTargetMethodExtractor<EfTargetMethodExtractor>());

        (this.dbContextInit ?? throw new InvalidOperationException("DbContext has not been initialized.")).Invoke(services);

        this.extensions.ForEach(ex => ex.AddServices(services));
    }

    public IEntityFrameworkSetup SetDbContext<TDbContext>()
        where TDbContext : DbContext
    {
        this.dbContextInit = sc =>
        {
            sc.AddScoped<IDBSession, EfSession<TDbContext>>();
            sc.AddScopedFrom((EfSession<TDbContext> efSession) => efSession.InnerSession);
        };

        return this;
    }

    public IEntityFrameworkSetup AddExtension(IEntityFrameworkSetupExtension extension)
    {
        this.extensions.Add(extension);

        return this;
    }
}
