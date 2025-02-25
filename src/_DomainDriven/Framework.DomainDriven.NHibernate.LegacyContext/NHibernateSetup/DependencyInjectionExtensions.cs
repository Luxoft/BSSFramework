using Framework.DependencyInjection;
using Framework.DomainDriven.Tracking;
using Framework.DomainDriven.DALExceptions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.NHibernate;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddLegacyDatabaseSettings(this IServiceCollection services)
    {
        services.AddScoped(typeof(IDAL<,>), typeof(NHibDal<,>));
        services.AddScoped<IObjectStateService, NHibObjectStatesService>();

        return services.ReplaceSingleton<IDalValidationIdentitySource, LegacyDalValidationIdentitySource>();
    }
}
