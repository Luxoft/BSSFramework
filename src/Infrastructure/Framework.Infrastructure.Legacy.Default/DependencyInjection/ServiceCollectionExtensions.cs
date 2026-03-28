using System.Reflection;

using CommonFramework.DependencyInjection;

using Framework.ApplicationVariable;
using Framework.Authorization.BLL;
using Framework.Authorization.Events;
using Framework.Authorization.Generated.DTO;
using Framework.BLL.DTOMapping.DTOMapper;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.Core.MessageSender;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.Setup;
using Framework.Events;
using Framework.Events.Legacy;
using Framework.Projection;
using Framework.QueryLanguage;
using Framework.Security;
using Framework.Tracking;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.DependencyInjection;
using SecuritySystem.DomainServices;
using SecuritySystem.DomainServices.DependencySecurity;

using HierarchicalExpand;

using SecuritySystem.SecurityRuleInfo;
using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions2
{
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterLegacyGenericServices()
        {
            services.AddScoped<IApplicationVariableStorage, ConfigurationApplicationVariableStorage>();
            services.AddScoped<IEventSystem, ConfigurationEventSystem>();

            services.AddSingleton<SubscriptionMetadataStore>();
            services.AddSingleton<ISubscriptionMetadataFinder, SubscriptionMetadataFinder>();
            services.AddScoped<ISubscriptionInitializer, SubscriptionInitializer>();

            services.AddScoped<ISystemConstantInitializer, SystemConstantInitializer>();

            services.AddSingleton(AuthDALListenerSettings.DefaultSettings);

            services.AddScoped<IAuthorizationDTOMappingService, AuthorizationServerPrimitiveDTOMappingService>();
            services.AddScoped<IConfigurationDTOMappingService, ConfigurationServerPrimitiveDTOMappingService>();

            services.AddKeyedScoped<IEventOperationSender, BLLEventOperationSender>("BLL");

            services.AddScoped(typeof(EvaluatedData<,>));

            services.AddSingleton<IStandardExpressionBuilder, StandardExpressionBuilder>();

            services.AddScoped<IStandardSubscriptionService, LocalDBSubscriptionService>();

            services.AddSingleton(AvailableValuesHelper.AvailableValues.ToValidation());

            services.AddSingleton<IPersistentInfoService, PersistentInfoService>();

            services.AddScoped<IRootSecurityService, RootSecurityService>();
            services.AddScoped(typeof(ITrackingService<>), typeof(TrackingService<>));

            services.RegisterAuthorizationBLL();
            services.RegisterConfigurationBLL();
            services.RegisterConfigurationNamedLocks();

            services.ReplaceSingleton<IActualDomainTypeResolver, ProjectionActualDomainTypeResolver>();
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

        private IServiceCollection RegisterAuthorizationBLL()
        {
            return services.RegisterBLLSystem<IAuthorizationBLLContext, AuthorizationBLLContext>();
        }

        private IServiceCollection RegisterConfigurationBLL()
        {
            return services
                   .RegisterBLLSystem<IConfigurationBLLContext, ConfigurationBLLContext>()
                   .AddScoped<ISubscriptionBLL, SubscriptionBLL>()

                   .AddScopedFrom<ICurrentRevisionService, IDBSession>()
                   .AddScoped<IMessageSender<Notification.MessageTemplateNotification>, TemplateMessageSender>()
                   .AddScoped<IMessageSender<Notification.DTO.NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>();
        }


        private IServiceCollection RegisterConfigurationNamedLocks()
        {
            return services.AddKeyedSingleton<INamedLockSource>(
                RootNamedLockSource.ElementsKey,
                new NamedLockTypeContainerSource(typeof(ConfigurationNamedLock)));
        }
    }


    public static ISecuritySystemBuilder AddConfigurationSecurity(this ISecuritySystemBuilder securitySystemSettings)
    {
        return securitySystemSettings
               .AddDomainSecurity<ExceptionMessage>(b => b.SetView(SecurityRole.Administrator))
               .AddDomainSecurity<Sequence>(b => b.SetView(SecurityRole.Administrator).SetEdit(SecurityRole.Administrator))
               .AddDomainSecurity<SystemConstant>(b => b.SetView(SecurityRole.Administrator).SetEdit(SecurityRole.Administrator))
               .AddDomainSecurity<CodeFirstSubscription>(b => b.SetView(SecurityRole.Administrator).SetEdit(SecurityRole.Administrator))
               .AddDomainSecurity<TargetSystem>(b => b.SetView(SecurityRole.Administrator).SetEdit(SecurityRole.Administrator))
               .AddDomainSecurity<DomainType>(b => b.SetView(SecurityRule.Disabled));
    }
}
