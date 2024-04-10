using Framework.Authorization.Notification;
using Framework.Configuration.Domain;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Events;
using Framework.SecuritySystem;
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

        foreach (var securityRoleType in settings.SecurityRoleTypes)
        {
            services.AddSingleton(new SecurityRoleTypeInfo(securityRoleType));
        }

        foreach (var namedLockType in settings.NamedLockTypes)
        {
            services.AddSingleton(new NamedLockTypeInfo(namedLockType));
        }

        if (settings.AdministratorRole != null)
        {
            services.AddSingleton(new AdministratorRoleInfo(settings.AdministratorRole));
        }

        if (settings.SystemIntegrationRole != null)
        {
            services.AddSingleton(new SystemIntegrationRoleInfo(settings.SystemIntegrationRole));
        }

        services.AddScoped(typeof(INotificationPrincipalExtractor), settings.NotificationPrincipalExtractorType);
        services.AddScoped(typeof(IDomainObjectEventMetadata), settings.DomainObjectEventMetadataType);

        settings.RegisterActions.ForEach(a => a(services));

        settings.Extensions.ForEach(ex => ex.AddServices(services));

        services.RegisterGenericServices();

        services.RegisterWebApiGenericServices();

        return services;
    }

    private static void InitSettings(this BssFrameworkSettings settings)
    {
        if (settings.RegisterBaseNamedLockTypes)
        {
            settings.NamedLockTypes.Add(typeof(ConfigurationNamedLock));
        }

        if (settings.RegisterDenormalizeHierarchicalDALListener)
        {
            settings.AddListener<DenormalizeHierarchicalDALListener>();
        }

        if (settings.NotificationPrincipalExtractorType == null)
        {
            settings.SetNotificationPrincipalExtractor<NotificationPrincipalExtractor>();
        }

        if (settings.DomainObjectEventMetadataType == null)
        {
            settings.SetDomainObjectEventMetadata<DomainObjectEventMetadata>();
        }
    }
}
