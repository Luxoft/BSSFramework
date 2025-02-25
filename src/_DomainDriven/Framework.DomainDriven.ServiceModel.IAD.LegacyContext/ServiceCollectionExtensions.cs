using System.Reflection;

using Framework.Authorization.BLL;
using Framework.Authorization.Events;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
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
using Framework.SecuritySystem.SecurityRuleInfo;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.Lock;
using Framework.SecuritySystem.DependencyInjection;
using Framework.ApplicationVariable;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLegacyGenericServices(this IServiceCollection services)
    {
        services.AddScoped<IApplicationVariableStorage, ConfigurationApplicationVariableStorage>();
        services.AddScoped<IEventSystem, ConfigurationEventSystem>();

        services.AddSingleton<SubscriptionMetadataStore>();
        services.AddSingleton<ISubscriptionMetadataFinder, SubscriptionMetadataFinder>();
        services.AddScoped<ISubscriptionInitializer, SubscriptionInitializer>();

        services.AddSingleton(AuthDALListenerSettings.DefaultSettings);

        services.AddScoped<IAuthorizationDTOMappingService, AuthorizationServerPrimitiveDTOMappingService>();
        services.AddScoped<IConfigurationDTOMappingService, ConfigurationServerPrimitiveDTOMappingService>();

        services.AddKeyedScoped<IEventOperationSender, BLLEventOperationSender>("BLL");

        services.AddScoped(typeof(EvaluatedData<,>));

        services.AddSingleton<IStandartExpressionBuilder, StandartExpressionBuilder>();

        services.AddScoped<IStandardSubscriptionService, LocalDBSubscriptionService>();

        services.AddSingleton(AvailableValuesHelper.AvailableValues.ToValidation());

        services.AddSingleton<IPersistentInfoService, PersistentInfoService>();

        services.AddScoped(typeof(IRootSecurityService<>), typeof(RootSecurityService<>));
        services.AddScoped(typeof(ITrackingService<>), typeof(TrackingService<>));

        services.RegisterAuthorizationBLL();
        services.RegisterConfigurationBLL();
        services.RegisterConfigurationSecurity();
        services.RegisterConfigurationNamedLocks();

        services.ReplaceSingleton<IRealTypeResolver, ProjectionRealTypeResolver>();
        services.ReplaceSingleton<ISecurityContextInfoSource, ProjectionSecurityContextInfoSource>();

        services
            .AddScoped<IDomainEventDTOMapper<Framework.Authorization.Domain.PersistentDomainObjectBase>,
                AuthorizationRuntimeDomainEventDTOMapper>();

        services.AddSingleton(
            new LocalDBEventMessageSenderSettings<Framework.Authorization.Domain.PersistentDomainObjectBase> { QueueTag = "authDALQuery" });

        services.AddScoped(typeof(IEventDTOMessageSender<>), typeof(LocalDBEventMessageSender<>));

        services.AddSingleton(typeof(RuntimeDomainEventDTOConverter<,,>));

        services
            .AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<
                Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>();

        return services;
    }

    private static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
    {
        return services.RegisterBLLSystem<IAuthorizationBLLContext, AuthorizationBLLContext>();
    }

    private static IServiceCollection RegisterConfigurationBLL(this IServiceCollection services)
    {
        return services
               .RegisterBLLSystem<IConfigurationBLLContext, ConfigurationBLLContext>()
               .AddScopedFrom<ICurrentRevisionService, IDBSession>()
               .AddScoped<IMessageSender<Notification.MessageTemplateNotification>, TemplateMessageSender>()
               .AddScoped<IMessageSender<Notification.DTO.NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>();
    }

    public static IServiceCollection RegisterProjectionDomainSecurityServices(this IServiceCollection services, Assembly assembly)
    {
        var projectionsRequest = from type in assembly.GetTypes()

                                 let projectionAttr = type.GetCustomAttribute<ProjectionAttribute>()

                                 where projectionAttr != null && type.HasAttribute<DependencySecurityAttribute>()

                                 select new
                                        {
                                            DomainType = type,
                                            projectionAttr.SourceType,
                                            CustomViewSecurityRule = (DomainSecurityRule)type.GetViewSecurityRule()
                                        };

        foreach (var pair in projectionsRequest)
        {
            services.AddScoped(
                typeof(IDomainSecurityService<>).MakeGenericType(pair.DomainType),
                typeof(UntypedDependencyDomainSecurityService<,>).MakeGenericType(pair.DomainType, pair.SourceType));

            if (pair.CustomViewSecurityRule != null)
            {
                services.AddSingleton(
                    new DomainModeSecurityRuleInfo(SecurityRule.View.ToDomain(pair.DomainType), pair.CustomViewSecurityRule));
            }
        }

        return services;
    }

    private static IServiceCollection RegisterConfigurationSecurity(this IServiceCollection services)
    {
        return services.RegisterDomainSecurityServices(

            rb => rb.Add<ExceptionMessage>(
                        b => b.SetView(SecurityRole.Administrator))

                    .Add<Sequence>(
                        b => b.SetView(SecurityRole.Administrator)
                              .SetEdit(SecurityRole.Administrator))

                    .Add<SystemConstant>(
                        b => b.SetView(SecurityRole.Administrator)
                              .SetEdit(SecurityRole.Administrator))

                    .Add<CodeFirstSubscription>(
                        b => b.SetView(SecurityRole.Administrator)
                              .SetEdit(SecurityRole.Administrator))

                    .Add<TargetSystem>(
                        b => b.SetView(SecurityRole.Administrator)
                              .SetEdit(SecurityRole.Administrator))

                    .Add<DomainType>(
                        b => b.SetView(SecurityRule.Disabled)));
    }

    private static IServiceCollection RegisterConfigurationNamedLocks(this IServiceCollection services)
    {
        return services.AddKeyedSingleton<INamedLockSource>(
            RootNamedLockSource.ElementsKey,
            new NamedLockTypeContainerSource(typeof(ConfigurationNamedLock)));
    }
}
