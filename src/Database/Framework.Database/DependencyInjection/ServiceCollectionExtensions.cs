using CommonFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeneralDatabase(this IServiceCollection services, Action<IDatabaseSetup>? setupAction = null) =>
        services.Initialize<DatabaseSetup>(setupAction);
}
