using Anch.DependencyInjection;

using Framework.Tracking;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLegacyEntityFrameworkSettings(this IServiceCollection services)
    {
        services.AddScoped(typeof(IDAL<,>), typeof(EfDal<,>));
        services.AddScoped<IObjectStateService, EfObjectStatesService>();

        return services.ReplaceSingleton<IDalValidationIdentitySource, LegacyDalValidationIdentitySource>();
    }
}
