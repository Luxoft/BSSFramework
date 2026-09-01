using Anch.DependencyInjection;
using Anch.GenericQueryable.DependencyInjection;
using Anch.GenericQueryable.EntityFramework;

using Framework.Core;
using Framework.Database.EntityFramework.Sessions;
using Framework.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.DependencyInjection;

public class EntityFrameworkSetup<TDbContext> : IEntityFrameworkSetup<TDbContext>, IServiceInitializer
    where TDbContext : DbContext
{
    private readonly List<IEntityFrameworkSetupExtension> extensions = [];

    public void Initialize(IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncDal<,>), typeof(EfAsyncDal<,>));

        services.AddGenericQueryable(v => v.SetFetchService<EfFetchService>().SetTargetMethodExtractor<EfTargetMethodExtractor>());

        services.AddScopedFrom<ILazyObject<IDBSession>, ILazyObject<IEfSession>>();
        services.AddScopedFrom<DbContext, IEfSession>(session => session.NativeSession);
        services.AddScopedFromLazyObject<IEfSession, EfSession<TDbContext>>();

        this.extensions.ForEach(ex => ex.AddServices(services));
    }

    public IEntityFrameworkSetup<TDbContext> AddExtension(IEntityFrameworkSetupExtension extension)
    {
        this.extensions.Add(extension);

        return this;
    }
}
