using System.Reflection;

using Framework.Authorization.BLL;
using Framework.Authorization.Notification;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Tracking;
using Framework.DomainDriven.NHibernate;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.Projection;
using Framework.QueryLanguage;
using Framework.Security;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLegacyGenericServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IOperationEventSenderContainer<>), typeof(OperationEventSenderContainer<>));

        services.AddScoped(typeof(IDAL<,>), typeof(NHibDal<,>));

        services.AddSingleton<IExceptionExpander, ExceptionExpander>();

        services.AddSingleton<IStandartExpressionBuilder, StandartExpressionBuilder>();

        services.AddScoped<IStandardSubscriptionService, LocalDBSubscriptionService>();

        services.AddSingleton(AvailableValuesHelper.AvailableValues.ToValidation());

        services.AddSingleton<IDateTimeService>(DateTimeService.Default);

        services.AddSingleton<IPersistentInfoService, PersistentInfoService>();

        services.RegisterAuthorizationBLL();
        services.RegisterConfigurationBLL();

        return services;
    }

    public static IServiceCollection RegisterLegacyHierarchicalObjectExpander(this IServiceCollection services)
    {
        return services.ReplaceSingleton<IRealTypeResolver, ProjectionRealTypeResolver>();
    }

    public static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
    {
        return services

               .AddSingleton<AuthorizationValidationMap>()
               .AddSingleton<AuthorizationValidatorCompileCache>()
               .AddScoped<IAuthorizationValidator, AuthorizationValidator>()

               .AddSingleton(new AuthorizationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Authorization.Domain.PersistentDomainObjectBase>.OData))
               .AddScoped<IAuthorizationSecurityService, AuthorizationSecurityService>()
               .AddScoped<IAuthorizationBLLFactoryContainer, AuthorizationBLLFactoryContainer>()
               //.AddScoped<INotificationPrincipalExtractor, LegacyNotificationPrincipalExtractor>()
               .AddScoped<INotificationBasePermissionFilterSource, LegacyNotificationPrincipalExtractor>()
               .AddScoped<IAuthorizationBLLContextSettings, AuthorizationBLLContextSettings>()

               .AddScopedFromLazyInterfaceImplement<IAuthorizationBLLContext, AuthorizationBLLContext>()

               .AddScoped<ITrackingService<Framework.Authorization.Domain.PersistentDomainObjectBase>, TrackingService<Framework.Authorization.Domain.PersistentDomainObjectBase>>()

               .Self(AuthorizationSecurityOperationHelper.RegisterDomainObjectSecurityOperations)
               .Self(AuthorizationSecurityServiceBase.Register)
               .Self(AuthorizationBLLFactoryContainer.RegisterBLLFactory);
    }

    public static IServiceCollection RegisterConfigurationBLL(this IServiceCollection services)
    {
        return services

               .AddSingleton<ConfigurationValidationMap>()
               .AddSingleton<ConfigurationValidatorCompileCache>()
               .AddScoped<IConfigurationValidator, ConfigurationValidator>()

               .AddSingleton(new ConfigurationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Configuration.Domain.PersistentDomainObjectBase>.OData))
               .AddScoped<IConfigurationSecurityService, ConfigurationSecurityService>()
               .AddScoped<IConfigurationBLLFactoryContainer, ConfigurationBLLFactoryContainer>()

               .AddScopedFrom<ICurrentRevisionService, IDBSession>()

               .AddScoped<IMessageSender<Framework.Notification.MessageTemplateNotification>, TemplateMessageSender>()
               .AddScoped<IMessageSender<Framework.Notification.DTO.NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>()

               .AddScoped<IConfigurationBLLContextSettings, ConfigurationBLLContextSettings>()
               .AddScopedFromLazyInterfaceImplement<IConfigurationBLLContext, ConfigurationBLLContext>()

               .AddScoped<ITrackingService<Framework.Configuration.Domain.PersistentDomainObjectBase>, TrackingService<Framework.Configuration.Domain.PersistentDomainObjectBase>>()

               .AddScopedFrom<Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContext, IConfigurationBLLContext>()

               .AddScopedFrom<IConfigurationSecurityPathContainer, IConfigurationSecurityService>()

               .Self(ConfigurationSecurityOperationHelper.RegisterDomainObjectSecurityOperations)
               .Self(ConfigurationSecurityServiceBase.Register)
               .Self(ConfigurationBLLFactoryContainer.RegisterBLLFactory);
    }

    public static IServiceCollection RegisterProjectionDomainSecurityServices(this IServiceCollection services, Assembly assembly)
    {
        var projectionsRequest = from type in assembly.GetTypes()

                                 let projectionAttr = type.GetCustomAttribute<ProjectionAttribute>()

                                 where projectionAttr != null && type.HasAttribute<DependencySecurityAttribute>()

                                 select new
                                        {
                                            DomainType = type,
                                            SourceType = projectionAttr.SourceType,
                                            CustomViewSecurityOperation = type.GetViewSecurityOperation()
                                        };

        foreach (var pair in projectionsRequest)
        {
            services.AddScoped(
                typeof(IDomainSecurityService<>).MakeGenericType(pair.DomainType),
                typeof(DependencyDomainSecurityService<,>).MakeGenericType(pair.DomainType, pair.SourceType));

            if (pair.CustomViewSecurityOperation != null)
            {
                services.AddSingleton(new DomainObjectSecurityOperationInfo(pair.DomainType, pair.CustomViewSecurityOperation, null));
            }
        }

        return services;
    }
}
