using Framework.Database.EntityFramework.DependencyInjection;

using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework;

public static class EntityFrameworkSetupObjectExtensions
{
    public static IEntityFrameworkSetup<TDbContext> AddLegacyDatabaseSettings<TDbContext>(this IEntityFrameworkSetup<TDbContext> setupObject)
        where TDbContext : DbContext => setupObject.AddExtension(new EntityFrameworkSetupExtension(services => services.AddLegacyEntityFrameworkSettings()));
}
