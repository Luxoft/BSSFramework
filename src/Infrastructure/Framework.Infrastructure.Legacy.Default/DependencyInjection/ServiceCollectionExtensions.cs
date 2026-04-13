using Framework.Application.ApplicationVariable;
using Framework.Application.Events;
using Framework.Application.Lock;
using Framework.Authorization.BLL;
using Framework.Authorization.Events;
using Framework.Authorization.Generated.DTO;
using Framework.BLL.DependencyInjection;
using Framework.BLL.DTOMapping.DTOMapper;
using Framework.BLL.Events.SubscriptionManager;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Jobs;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.Infrastructure.LocalDBEvents;
using Framework.Infrastructure.SubscriptionService;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddNotificationJob() => services.AddScoped<ISendNotificationsJob, SendNotificationsJob>();

        public IServiceCollection AddLegacyDefaultGenericServices()
        {
            services.AddScoped<IApplicationVariableStorage, ConfigurationApplicationVariableStorage>();
            services.AddScoped<IEventSystem, ConfigurationEventSystem>();

            services.AddScoped<IDomainObjectVersionsResolverFactory, DomainObjectVersionsResolverFactory>();

            services.AddScoped<ISystemConstantInitializer, SystemConstantInitializer>();

            services.AddSingleton(AuthDALListenerSettings.DefaultSettings);

            services.AddScoped<IAuthorizationDTOMappingService, AuthorizationServerPrimitiveDTOMappingService>();
            services.AddScoped<IConfigurationDTOMappingService, ConfigurationServerPrimitiveDTOMappingService>();

            services.AddScoped<IObjectModificationProcessor, LocalDbObjectModificationProcessor>();

            services.AddAuthorizationBLL();
            services.AddConfigurationBLL();
            services.AddConfigurationNamedLocks();

            services
                .AddScoped<IDomainEventDTOMapper<Framework.Authorization.Domain.PersistentDomainObjectBase>,
                    AuthorizationRuntimeDomainEventDTOMapper>();

            services.AddSingleton(
                new LocalDBEventMessageSenderSettings<Framework.Authorization.Domain.PersistentDomainObjectBase> { QueueTag = "authDALQuery" });

            services.AddScoped(typeof(IEventDTOMessageSender<>), typeof(LocalDBEventMessageSender<>));

            services.AddSingleton(typeof(RuntimeDomainEventDTOConverter<,,>));

            //services
            //    .AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<
            //        Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>();

            return services;
        }

        private IServiceCollection AddAuthorizationBLL() => services.AddBLLSystem<IAuthorizationBLLContext, AuthorizationBLLContext>();

        private IServiceCollection AddConfigurationBLL() =>
            services
                .AddBLLSystem<IConfigurationBLLContext, ConfigurationBLLContext>()
                .AddKeyedSingleton<ISerializerFactory<string>>(nameof(SystemConstant), SerializerFactory.Default)

                .AddScoped<IMessageSender<Notification.Domain.Notification>, LocalDbNotificationMessageSender>();

        private IServiceCollection AddConfigurationNamedLocks() =>
            services.AddKeyedSingleton<INamedLockSource>(
                RootNamedLockSource.ElementsKey,
                new NamedLockTypeContainerSource(typeof(ConfigurationNamedLock)));
    }


    public static ISecuritySystemSetup AddConfigurationSecurity(this ISecuritySystemSetup securitySystemSettings) =>
        securitySystemSettings
            .AddDomainSecurity<Sequence>(b => b.SetView(SecurityRole.Administrator).SetEdit(SecurityRole.Administrator))
            .AddDomainSecurity<SystemConstant>(b => b.SetView(SecurityRole.Administrator).SetEdit(SecurityRole.Administrator))
            .AddDomainSecurity<TargetSystem>(b => b.SetView(SecurityRole.Administrator).SetEdit(SecurityRole.Administrator))
            .AddDomainSecurity<DomainType>(b => b.SetView(SecurityRule.Disabled));
}
