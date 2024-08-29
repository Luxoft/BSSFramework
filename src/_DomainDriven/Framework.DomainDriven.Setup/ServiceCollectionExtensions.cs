using Framework.DomainDriven.ApplicationCore;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBssFramework(this IServiceCollection services, Action<IBssFrameworkSettings> setupAction)
    {
        services.RegisterGenericServices();
        services.RegisterWebApiGenericServices();

        var settings = new BssFrameworkSettings();

        setupAction?.Invoke(settings);
        settings.TryInitDefault();
        settings.Init();

        services.AddSingleton(new SecurityAdministratorRuleInfo(settings.SecurityAdministratorRule));

        foreach (var namedLockType in settings.NamedLockTypes)
        {
            services.AddSingleton(new NamedLockTypeInfo(namedLockType));
        }

        services.AddScoped(typeof(IDomainObjectEventMetadata), settings.DomainObjectEventMetadataType);

        settings.RegisterActions.ForEach(a => a(services));

        settings.Extensions.ForEach(ex => ex.AddServices(services));

        return services;
    }
}
