using Anch.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGeneralDatabase(this IServiceCollection services, Action<IDatabaseSetup>? setupAction = null) =>
        services.Initialize<DatabaseSetup>(setupAction);

    public static IServiceCollection AddDatabaseVisitors(this IServiceCollection services, Action<IDatabaseVisitorSetup>? setupAction = null) =>
        services.Initialize<DatabaseVisitorSetup>(setupAction);
}
