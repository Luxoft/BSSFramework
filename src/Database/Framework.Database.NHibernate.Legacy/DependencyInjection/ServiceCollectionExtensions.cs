using CommonFramework.DependencyInjection;

using Framework.Tracking;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.NHibernate.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLegacyNHibernateSettings(this IServiceCollection services)
    {
        services.AddScoped(typeof(IDAL<,>), typeof(NHibDal<,>));
        services.AddScoped<IObjectStateService, NHibObjectStatesService>();

        return services.ReplaceSingleton<IDalValidationIdentitySource, LegacyDalValidationIdentitySource>();
    }
}
