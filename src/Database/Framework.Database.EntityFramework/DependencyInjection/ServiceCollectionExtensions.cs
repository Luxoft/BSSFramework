using Anch.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFramework<TDbContext>(this IServiceCollection services, Action<IEntityFrameworkSetup<TDbContext>>? setupAction = null)
        where TDbContext : DbContext => services.Initialize<EntityFrameworkSetup<TDbContext>>(setupAction);
}
