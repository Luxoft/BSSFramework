﻿using Framework.Authorization.Environment.Security;
using Framework.Authorization.Notification;
using Framework.Configuration.Domain;
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
        var settings = new BssFrameworkSettings();

        setupAction?.Invoke(settings);
        settings.InitSettings();

        services.AddSingleton(new SecurityAdministratorRuleInfo(settings.SecurityAdministratorRule));

        foreach (var namedLockType in settings.NamedLockTypes)
        {
            services.AddSingleton(new NamedLockTypeInfo(namedLockType));
        }

        services.AddScoped<INotificationBasePermissionFilterSource, NotificationBasePermissionFilterSource>();
        services.AddScoped(typeof(INotificationPrincipalExtractor), settings.NotificationPrincipalExtractorType);

        services.AddScoped(typeof(IDomainObjectEventMetadata), settings.DomainObjectEventMetadataType);

        services.RegisterGenericServices();

        services.RegisterWebApiGenericServices();
        settings.RegisterActions.ForEach(a => a(services));

        settings.Extensions.ForEach(ex => ex.AddServices(services));

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
