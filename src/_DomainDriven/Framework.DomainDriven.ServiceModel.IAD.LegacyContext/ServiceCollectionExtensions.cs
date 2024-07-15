using System.Reflection;

using Framework.Authorization.BLL;
using Framework.Authorization.Events;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using Framework.DomainDriven.NHibernate;
using Framework.HierarchicalExpand;
using Framework.Projection;
using Framework.QueryLanguage;
using Framework.Security;
using Framework.SecuritySystem;
using Framework.Events.Legacy;
using Framework.Events;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration.Generated.DTO;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.DomainDriven.Setup;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLegacyGenericServices(this IServiceCollection services)
    {
        services.AddScopedFrom<ILegacyForceEventSystem, IDomainTypeBLLFactory>(domainTypeBllFactory => domainTypeBllFactory.Create());

        services.AddSingleton<SubscriptionMetadataStore>();
        services.AddSingleton<ISubscriptionMetadataFinder, SubscriptionMetadataFinder>();

        services.AddSingleton(AuthDALListenerSettings.DefaultSettings);

        services.AddScoped<IAuthorizationDTOMappingService, AuthorizationServerPrimitiveDTOMappingService>();
        services.AddScoped<IConfigurationDTOMappingService, ConfigurationServerPrimitiveDTOMappingService>();

        services.AddKeyedScoped<IEventOperationSender, BLLEventOperationSender>("BLL");

        services.AddScoped(typeof(EvaluatedData<,>));

        services.AddScoped(typeof(IDAL<,>), typeof(NHibDal<,>));

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

        services.AddSingleton(new LocalDBEventMessageSenderSettings<Framework.Authorization.Domain.PersistentDomainObjectBase> { QueueTag = "authDALQuery" });

        services.AddScoped(typeof(IEventDTOMessageSender<>), typeof(LocalDBEventMessageSender<>));

        services.AddSingleton(typeof(RuntimeDomainEventDTOConverter<,,>));

        return services;
    }

    public static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
    {
        return services.RegisterBLLSystem<IAuthorizationBLLContext, AuthorizationBLLContext>();
    }

    public static IServiceCollection RegisterConfigurationBLL(this IServiceCollection services)
    {
        return services
               .RegisterBLLSystem<IConfigurationBLLContext, ConfigurationBLLContext>()
               .AddScopedFrom<ICurrentRevisionService, IDBSession>()
               .AddScoped<IMessageSender<Framework.Notification.MessageTemplateNotification>, TemplateMessageSender>()
               .AddScoped<IMessageSender<Framework.Notification.DTO.NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>();
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
                                            CustomViewSecurityRule = (SecurityRule.DomainSecurityRule)type.GetViewSecurityRule()
                                        };

        foreach (var pair in projectionsRequest)
        {
            services.AddScoped(
                typeof(IDomainSecurityService<>).MakeGenericType(pair.DomainType),
                typeof(UntypedDependencyDomainSecurityService<,,>).MakeGenericType(pair.DomainType, pair.SourceType, typeof(Guid)));

            if (pair.CustomViewSecurityRule != null)
            {
                services.AddSingleton(new DomainObjectSecurityModeInfo(pair.DomainType, pair.CustomViewSecurityRule, null));
            }
        }

        return services;
    }
}
