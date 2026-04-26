using Anch.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, Action<IEntityFrameworkSetup>? setupAction = null) =>
        services.Initialize<EntityFrameworkSetup>(setupAction);
}
