using Framework.Authorization;
using Framework.Authorization.SecuritySystem;
using Framework.Configuration;
using Framework.Configuration.Domain;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;
using Framework.SecuritySystem.Bss;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBssFramework(this IServiceCollection services, Action<IBssFrameworkSettings> setupAction)
    {
        var settings = new BssFrameworkSettings();

        setupAction?.Invoke(settings);
        settings.InitSettings();

        foreach (var securityOperationType in settings.SecurityOperationTypes)
        {
            services.AddSingleton(new SecurityOperationTypeInfo(securityOperationType));
        }

        foreach (var securityRoleType in settings.SecurityRoleTypes)
        {
            services.AddSingleton(new SecurityRoleTypeInfo(securityRoleType));
        }

        foreach (var namedLockType in settings.NamedLockTypes)
        {
            services.AddSingleton(new NamedLockTypeInfo(namedLockType));
        }

        settings.RegisterSecurityContextActions.ForEach(a => a(services));

        settings.RegisterDomainSecurityServicesActions.ForEach(a => a(services));

        settings.Extensions.ForEach(ex => ex.AddServices(services));

        services.RegisterGenericServices();

        services.RegisterWebApiGenericServices();

        return services;
    }

    private static void InitSettings(this BssFrameworkSettings settings)
    {
        if (settings.RegisterBaseSecurityOperationTypes)
        {
            settings.SecurityOperationTypes.Add(typeof(BssSecurityOperation));
            settings.SecurityOperationTypes.Add(typeof(AuthorizationSecurityOperation));
            settings.SecurityOperationTypes.Add(typeof(ConfigurationSecurityOperation));
        }

        if (settings.RegisterBaseNamedLockTypes)
        {
            settings.NamedLockTypes.Add(typeof(ConfigurationNamedLock));
        }
    }
}
