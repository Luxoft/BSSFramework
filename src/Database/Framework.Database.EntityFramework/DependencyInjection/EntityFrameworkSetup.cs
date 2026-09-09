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

    private readonly List<Type> secondaryDbContextTypes = [];

    public void Initialize(IServiceCollection services)
    {
        if (this.secondaryDbContextTypes.Count == 0)
        {
            services.AddScoped(typeof(IAsyncDal<,>), typeof(EfAsyncDal<,>));
        }
        else
        {
            foreach (var secondaryDbContextType in this.secondaryDbContextTypes)
            {
                services.AddKeyedSingleton(IDbContextTypeSource.SecondaryKey, secondaryDbContextType);
            }

            services.AddScoped<IDbContextTypeSource, DbContextTypeSource<TDbContext>>();
            services.AddSingleton<DbContextTypeSourceState>();

            services.AddKeyedScoped(typeof(IAsyncDal<,>), IDbContextTypeSource.PrimaryKey, typeof(EfAsyncDal<,>));
            services.AddScoped(typeof(SecondaryEfAsyncDal<,,>));
            services.AddScoped(typeof(IAsyncDal<,>), typeof(ComplexAsyncDal<,>));

            services.AddScoped(typeof(EfSession<>));
        }

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

    public IEntityFrameworkSetup<TDbContext> AddSecondaryContext<TSecondaryDbContext>()
        where TSecondaryDbContext : DbContext
    {
        this.secondaryDbContextTypes.Add(typeof(TSecondaryDbContext));

        return this;
    }
}
