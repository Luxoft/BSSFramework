using System.Reflection;

using Framework.Authorization.BLL;
using Framework.Authorization.Events;
using Framework.Authorization.Notification;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using Framework.DomainDriven.NHibernate;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.Projection;
using Framework.QueryLanguage;
using Framework.Security;
using Framework.SecuritySystem;
using Framework.Events.DTOMapper;
using Framework.Events;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLegacyGenericServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(EvaluatedData<,>));

        services.AddKeyedScoped<IEventOperationSender, EventOperationSender>("DAL");
        services.AddKeyedScoped<IEventOperationSender, EventOperationSender>("AuthBLL");
        services.AddKeyedScoped<IEventOperationSender, EventOperationSender>("ConfigBLL");
        services.AddKeyedScoped<IEventOperationSender, EventOperationSender>("MainBLL");
        services.AddKeyedScoped<IEventOperationSender, EventOperationSender>("Force");

        services.AddScoped(typeof(IDAL<,>), typeof(NHibDal<,>));

        services.AddSingleton<IExceptionExpander, ExceptionExpander>();

        services.AddSingleton<IStandartExpressionBuilder, StandartExpressionBuilder>();

        services.AddScoped<IStandardSubscriptionService, LocalDBSubscriptionService>();

        services.AddSingleton(AvailableValuesHelper.AvailableValues.ToValidation());

        services.AddSingleton<IPersistentInfoService, PersistentInfoService>();

        services.AddScoped(typeof(IRootSecurityService<>), typeof(RootSecurityService<>));
        services.AddScoped(typeof(ITrackingService<>), typeof(TrackingService<>));

        services.RegisterAuthorizationBLL();
        services.RegisterConfigurationBLL();

        services.ReplaceSingleton<IRealTypeResolver, ProjectionRealTypeResolver>();
        services.ReplaceSingleton<ISecurityContextInfoService, ProjectionSecurityContextInfoService>();

        services.AddScoped<IDomainEventDTOMapper<Framework.Authorization.Domain.PersistentDomainObjectBase>, AuthorizationRuntimeDomainEventDTOMapper>();

        services.AddSingleton(typeof(LocalDBEventMessageSenderSettings<>));
        services.AddSingleton(new LocalDBEventMessageSenderSettings<Framework.Authorization.Domain.PersistentDomainObjectBase> { QueueTag = "authDALQuery" });

        services.AddScoped(typeof(IEventDTOMessageSender<>), typeof(LocalDBEventMessageSender<>));

        services.AddSingleton(typeof(RuntimeDomainEventDTOConverter<,,>));

        return services;
    }

    public static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
    {
        return services

               .AddSingleton<AuthorizationValidationMap>()
               .AddSingleton<AuthorizationValidatorCompileCache>()
               .AddScoped<IAuthorizationValidator, AuthorizationValidator>()

               .AddSingleton(
                   new AuthorizationMainFetchService().WithCompress().WithCache().WithLock().Add(
                       FetchService<Framework.Authorization.Domain.PersistentDomainObjectBase>.OData))

               .AddScoped<IAuthorizationBLLFactoryContainer, AuthorizationBLLFactoryContainer>()

               //.AddScoped<INotificationPrincipalExtractor, LegacyNotificationPrincipalExtractor>()
               .AddScoped<INotificationBasePermissionFilterSource, LegacyNotificationPrincipalExtractor>()
               .AddScoped<IAuthorizationBLLContextSettings, AuthorizationBLLContextSettings>()

               .AddScopedFromLazyInterfaceImplement<IAuthorizationBLLContext, AuthorizationBLLContext>()

               .Self(AuthorizationBLLFactoryContainer.RegisterBLLFactory);
    }

    public static IServiceCollection RegisterConfigurationBLL(this IServiceCollection services)
    {
        return services

               .AddSingleton<ConfigurationValidationMap>()
               .AddSingleton<ConfigurationValidatorCompileCache>()
               .AddScoped<IConfigurationValidator, ConfigurationValidator>()

               .AddSingleton(
                   new ConfigurationMainFetchService().WithCompress().WithCache().WithLock().Add(
                       FetchService<Framework.Configuration.Domain.PersistentDomainObjectBase>.OData))
               .AddScoped<IConfigurationBLLFactoryContainer, ConfigurationBLLFactoryContainer>()

               .AddScopedFrom<ICurrentRevisionService, IDBSession>()

               .AddScoped<IMessageSender<Framework.Notification.MessageTemplateNotification>, TemplateMessageSender>()
               .AddScoped<IMessageSender<Framework.Notification.DTO.NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>()

               .AddScoped<IConfigurationBLLContextSettings, ConfigurationBLLContextSettings>()
               .AddScopedFromLazyInterfaceImplement<IConfigurationBLLContext, ConfigurationBLLContext>()

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
                typeof(UntypedDependencyDomainSecurityService<,,>).MakeGenericType(pair.DomainType, pair.SourceType, typeof(Guid)));

            if (pair.CustomViewSecurityOperation != null)
            {
                services.AddSingleton(new DomainObjectSecurityOperationInfo(pair.DomainType, pair.CustomViewSecurityOperation, null));
            }
        }

        return services;
    }
}
